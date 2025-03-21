﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Extensions;
using NGitLab.Models;

namespace NGitLab.Impl;

public class ProjectClient : IProjectClient
{
    private readonly API _api;

    public const string Url = Project.Url;

    public ProjectClient(API api)
    {
        _api = api;
    }

    public IEnumerable<Project> Accessible => _api.Get().GetAll<Project>(Utils.AddOrderBy($"{Project.Url}?membership=true"));

    public IEnumerable<Project> Owned => _api.Get().GetAll<Project>(Utils.AddOrderBy($"{Project.Url}?owned=true"));

    public IEnumerable<Project> Visible => _api.Get().GetAll<Project>(Utils.AddOrderBy(Project.Url));

    public Project this[long id] => GetById(id, new SingleProjectQuery());

    public Project Create(ProjectCreate project) => _api.Post().With(project).To<Project>(Project.Url);

    public Task<Project> CreateAsync(ProjectCreate project, CancellationToken cancellationToken = default) =>
        _api.Post().With(project).ToAsync<Project>(Project.Url, cancellationToken);

    public Project this[string fullName] => _api.Get().To<Project>($"{Project.Url}/{WebUtility.UrlEncode(fullName)}");

    public void Delete(long id) =>
        _api.Delete().Execute($"{Project.Url}/{id.ToString(CultureInfo.InvariantCulture)}");

    public Task DeleteAsync(ProjectId projectId, CancellationToken cancellationToken = default) =>
        _api.Delete().ExecuteAsync($"{Project.Url}/{projectId.ValueAsUriParameter()}", cancellationToken);

    public void Archive(long id) => _api.Post().Execute($"{Project.Url}/{id.ToString(CultureInfo.InvariantCulture)}/archive");

    public void Unarchive(long id) => _api.Post().Execute($"{Project.Url}/{id.ToString(CultureInfo.InvariantCulture)}/unarchive");

    private static bool SupportKeysetPagination(ProjectQuery query)
    {
        return string.IsNullOrEmpty(query.Search);
    }

    private static string CreateSearchUrl(ProjectQuery query)
    {
        var url = Project.Url;

        if (query.UserId.HasValue)
        {
            url = $"/users/{query.UserId.Value.ToStringInvariant()}/projects";
        }

        if (query.LastActivityAfter.HasValue)
        {
            url = Utils.AddParameter(url, "last_activity_after", query.LastActivityAfter.Value.UtcDateTime.ToString("O"));
        }

        switch (query.Scope)
        {
            case ProjectQueryScope.Accessible:
                url = Utils.AddParameter(url, "membership", value: true);
                break;
            case ProjectQueryScope.Owned:
                url = Utils.AddParameter(url, "owned", value: true);
                break;
            case ProjectQueryScope.All:
                // This is the default, it returns all visible projects.
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(query), $"'{nameof(query.Scope)}' has unknown value '{query.Scope}'");
        }

        url = Utils.AddParameter(url, "archived", query.Archived);
        url = Utils.AddOrderBy(url, query.OrderBy, supportKeysetPagination: SupportKeysetPagination(query));
        url = Utils.AddParameter(url, "search", query.Search);
        url = Utils.AddParameter(url, "simple", query.Simple);
        url = Utils.AddParameter(url, "statistics", query.Statistics);
        url = Utils.AddParameter(url, "per_page", query.PerPage);

        if (query.Ascending == true)
        {
            url = Utils.AddParameter(url, "sort", "asc");
        }

        if (query.Visibility.HasValue)
        {
            url = Utils.AddParameter(url, "visibility", query.Visibility.ToString().ToLowerInvariant());
        }

        if (query.MinAccessLevel != null)
        {
            url = Utils.AddParameter(url, "min_access_level", (int)query.MinAccessLevel.Value);
        }

        if (query.Topics.Any())
        {
            url = Utils.AddParameter(url, "topic", string.Join(",", query.Topics.Where(t => !string.IsNullOrWhiteSpace(t))));
        }

        return url;
    }

    public IEnumerable<Project> Get(ProjectQuery query)
    {
        var url = CreateSearchUrl(query);
        return _api.Get().GetAll<Project>(url);
    }

    public GitLabCollectionResponse<Project> GetAsync(ProjectQuery query)
    {
        var url = CreateSearchUrl(query);
        return _api.Get().GetAllAsync<Project>(url);
    }

    public Project GetById(long id, SingleProjectQuery query)
    {
        var url = $"{Project.Url}/{id.ToStringInvariant()}";
        if (query?.Statistics != null)
        {
            url = Utils.AddParameter(url, "statistics", query.Statistics);
        }

        return _api.Get().To<Project>(url);
    }

    public Task<Project> GetByIdAsync(long id, SingleProjectQuery query, CancellationToken cancellationToken = default) =>
        GetAsync(new ProjectId(id), query, cancellationToken);

    public Task<Project> GetByNamespacedPathAsync(string path, SingleProjectQuery query = null, CancellationToken cancellationToken = default) =>
        GetAsync(new ProjectId(path), query, cancellationToken);

    public async Task<Project> GetAsync(ProjectId projectId, SingleProjectQuery query = null, CancellationToken cancellationToken = default)
    {
        var url = $"{Project.Url}/{projectId.ValueAsUriParameter()}";
        if (query?.Statistics != null)
        {
            url = Utils.AddParameter(url, "statistics", query.Statistics);
        }

        return await _api.Get().ToAsync<Project>(url, cancellationToken).ConfigureAwait(false);
    }

    public Project Fork(string id, ForkProject forkProject)
    {
        return _api.Post().With(forkProject).To<Project>($"{Project.Url}/{id}/fork");
    }

    public Task<Project> ForkAsync(string id, ForkProject forkProject, CancellationToken cancellationToken = default)
    {
        return _api.Post().With(forkProject).ToAsync<Project>($"{Project.Url}/{id}/fork", cancellationToken);
    }

    public IEnumerable<Project> GetForks(string id, ForkedProjectQuery query)
    {
        var url = CreateGetForksUrl(id, query);
        return _api.Get().GetAll<Project>(url);
    }

    public GitLabCollectionResponse<Project> GetForksAsync(string id, ForkedProjectQuery query)
    {
        var url = CreateGetForksUrl(id, query);
        return _api.Get().GetAllAsync<Project>(url);
    }

    public GitLabCollectionResponse<Group> GetGroupsAsync(ProjectId id, ProjectGroupsQuery query)
    {
        var url = CreateGetGroupsUrl(id, query);
        return _api.Get().GetAllAsync<Group>(url);
    }

    public GitLabCollectionResponse<ProjectTemplate> GetProjectTemplatesAsync(ProjectId projectId, ProjectTemplateType projectTemplateType)
    {
        return _api.Get().GetAllAsync<ProjectTemplate>($"{Project.Url}/{projectId.ValueAsUriParameter()}/templates/{Utils.ToValueString(projectTemplateType)}");
    }

    public Task<ProjectMergeRequestTemplate> GetProjectMergeRequestTemplateAsync(ProjectId projectId, string name, CancellationToken cancellationToken = default)
    {
        return _api.Get().ToAsync<ProjectMergeRequestTemplate>($"{Project.Url}/{projectId.ValueAsUriParameter()}/templates/{Utils.ToValueString(ProjectTemplateType.MergeRequests)}/{name}", cancellationToken);
    }

    private static string CreateGetGroupsUrl(ProjectId projectId, ProjectGroupsQuery query)
    {
        var url = $"{Url}/{projectId.ValueAsUriParameter()}/groups";

        if (query is null)
        {
            return url;
        }

        if (query.SharedMinAccessLevel is not null)
        {
            url = Utils.AddParameter(url, "shared_min_access_level", (int)query.SharedMinAccessLevel);
        }

        if (query.SkipGroups is not null)
        {
            url = Utils.AddParameter(url, "skip_groups", query.SkipGroups);
        }

        url = Utils.AddParameter(url, "shared_visible_only", query.SharedVisibleOnly);
        url = Utils.AddParameter(url, "search", query.Search);
        url = Utils.AddParameter(url, "with_shared", query.WithShared);

        return url;
    }

    private static string CreateGetForksUrl(string id, ForkedProjectQuery query)
    {
        var url = $"{Project.Url}/{id}/forks";

        if (query != null)
        {
            url = Utils.AddParameter(url, "owned", query.Owned);
            url = Utils.AddParameter(url, "archived", query.Archived);
            url = Utils.AddParameter(url, "membership", query.Membership);
            url = Utils.AddOrderBy(url, query.OrderBy);
            url = Utils.AddParameter(url, "search", query.Search);
            url = Utils.AddParameter(url, "simple", query.Simple);
            url = Utils.AddParameter(url, "statistics", query.Statistics);
            url = Utils.AddParameter(url, "per_page", query.PerPage);

            if (query.Visibility.HasValue)
            {
                url = Utils.AddParameter(url, "visibility", query.Visibility.ToString().ToLowerInvariant());
            }

            if (query.MinAccessLevel != null)
            {
                url = Utils.AddParameter(url, "min_access_level", (int)query.MinAccessLevel.Value);
            }
        }

        return url;
    }

    public Dictionary<string, double> GetLanguages(string id)
    {
        var languages = DoGetLanguages(id);

        // After upgrading from v 11.6.2-ee to v 11.10.4-ee, the project /languages endpoint takes time execute.
        // So now we wait for the languages to be returned with a max wait time of 10 s.
        // The waiting logic should be removed once GitLab fix the issue in a version > 11.10.4-ee.
        var started = DateTime.UtcNow;
        while (languages.Count == 0 && (DateTime.UtcNow - started) < TimeSpan.FromSeconds(10))
        {
            Thread.Sleep(1000);
            languages = DoGetLanguages(id);
        }

        return languages;
    }

    private Dictionary<string, double> DoGetLanguages(string id)
    {
        return _api.Get().To<Dictionary<string, double>>($"{Project.Url}/{id}/languages");
    }

    public Project Update(string id, ProjectUpdate projectUpdate) =>
        _api.Put().With(projectUpdate).To<Project>($"{Project.Url}/{Uri.EscapeDataString(id)}");

    public Task<Project> UpdateAsync(ProjectId projectId, ProjectUpdate projectUpdate, CancellationToken cancellationToken = default) =>
        _api.Put().With(projectUpdate).ToAsync<Project>($"{Project.Url}/{projectId.ValueAsUriParameter()}", cancellationToken);

    public UploadedProjectFile UploadFile(string id, FormDataContent data)
        => _api.Post().With(data).To<UploadedProjectFile>($"{Project.Url}/{Uri.EscapeDataString(id)}/uploads");
}
