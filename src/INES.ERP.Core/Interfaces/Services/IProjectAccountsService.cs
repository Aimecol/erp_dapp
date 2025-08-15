using INES.ERP.Core.Models.ProjectAccounts;
using INES.ERP.Core.Models.Common;
using INES.ERP.Core.Enums;

namespace INES.ERP.Core.Interfaces.Services;

/// <summary>
/// Project accounts service interface
/// </summary>
public interface IProjectAccountsService
{
    #region Project Management

    /// <summary>
    /// Creates a new project
    /// </summary>
    /// <param name="project">Project to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created project</returns>
    Task<Project> CreateProjectAsync(Project project, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing project
    /// </summary>
    /// <param name="project">Project to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated project</returns>
    Task<Project> UpdateProjectAsync(Project project, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a project by ID
    /// </summary>
    /// <param name="projectId">Project ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Project or null if not found</returns>
    Task<Project?> GetProjectByIdAsync(Guid projectId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a project by code
    /// </summary>
    /// <param name="projectCode">Project code</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Project or null if not found</returns>
    Task<Project?> GetProjectByCodeAsync(string projectCode, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets projects with pagination and filtering
    /// </summary>
    /// <param name="pageNumber">Page number</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="searchTerm">Search term</param>
    /// <param name="projectType">Project type filter</param>
    /// <param name="status">Status filter</param>
    /// <param name="fundingSource">Funding source filter</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated projects</returns>
    Task<PagedResult<Project>> GetProjectsAsync(
        int pageNumber = 1,
        int pageSize = 50,
        string? searchTerm = null,
        string? projectType = null,
        Status? status = null,
        string? fundingSource = null,
        CancellationToken cancellationToken = default);

    #endregion

    #region Milestone Management

    /// <summary>
    /// Creates a project milestone
    /// </summary>
    /// <param name="milestone">Milestone to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created milestone</returns>
    Task<ProjectMilestone> CreateMilestoneAsync(ProjectMilestone milestone, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a project milestone
    /// </summary>
    /// <param name="milestone">Milestone to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated milestone</returns>
    Task<ProjectMilestone> UpdateMilestoneAsync(ProjectMilestone milestone, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets milestones for a project
    /// </summary>
    /// <param name="projectId">Project ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Project milestones</returns>
    Task<IEnumerable<ProjectMilestone>> GetProjectMilestonesAsync(Guid projectId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Marks a milestone as completed
    /// </summary>
    /// <param name="milestoneId">Milestone ID</param>
    /// <param name="completionDate">Completion date</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successful</returns>
    Task<bool> CompleteMilestoneAsync(Guid milestoneId, DateTime completionDate, CancellationToken cancellationToken = default);

    #endregion

    #region Disbursement Management

    /// <summary>
    /// Creates a project disbursement
    /// </summary>
    /// <param name="disbursement">Disbursement to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created disbursement</returns>
    Task<ProjectDisbursement> CreateDisbursementAsync(ProjectDisbursement disbursement, CancellationToken cancellationToken = default);

    /// <summary>
    /// Approves a disbursement
    /// </summary>
    /// <param name="disbursementId">Disbursement ID</param>
    /// <param name="approvedBy">User who approved</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successful</returns>
    Task<bool> ApproveDisbursementAsync(Guid disbursementId, string approvedBy, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets disbursements for a project
    /// </summary>
    /// <param name="projectId">Project ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Project disbursements</returns>
    Task<IEnumerable<ProjectDisbursement>> GetProjectDisbursementsAsync(Guid projectId, CancellationToken cancellationToken = default);

    #endregion

    #region Expense Management

    /// <summary>
    /// Records a project expense
    /// </summary>
    /// <param name="expense">Expense to record</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Recorded expense</returns>
    Task<ProjectExpense> RecordExpenseAsync(ProjectExpense expense, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets expenses for a project
    /// </summary>
    /// <param name="projectId">Project ID</param>
    /// <param name="fromDate">From date filter</param>
    /// <param name="toDate">To date filter</param>
    /// <param name="category">Category filter</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Project expenses</returns>
    Task<IEnumerable<ProjectExpense>> GetProjectExpensesAsync(
        Guid projectId,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        string? category = null,
        CancellationToken cancellationToken = default);

    #endregion

    #region Financial Analysis

    /// <summary>
    /// Gets project financial summary
    /// </summary>
    /// <param name="projectId">Project ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Financial summary</returns>
    Task<ProjectFinancialSummary> GetProjectFinancialSummaryAsync(Guid projectId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets project profitability analysis
    /// </summary>
    /// <param name="projectId">Project ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Profitability analysis</returns>
    Task<ProjectProfitabilityAnalysis> GetProjectProfitabilityAsync(Guid projectId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets budget utilization report
    /// </summary>
    /// <param name="projectId">Project ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Budget utilization</returns>
    Task<ProjectBudgetUtilization> GetBudgetUtilizationAsync(Guid projectId, CancellationToken cancellationToken = default);

    #endregion

    #region Reporting

    /// <summary>
    /// Generates project status report
    /// </summary>
    /// <param name="projectId">Project ID</param>
    /// <param name="reportDate">Report date</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Status report</returns>
    Task<ProjectStatusReport> GenerateStatusReportAsync(Guid projectId, DateTime reportDate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets overdue projects
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Overdue projects</returns>
    Task<IEnumerable<Project>> GetOverdueProjectsAsync(CancellationToken cancellationToken = default);

    #endregion
}

/// <summary>
/// Project financial summary
/// </summary>
public class ProjectFinancialSummary
{
    public Project Project { get; set; } = null!;
    public decimal TotalBudget { get; set; }
    public decimal TotalDisbursed { get; set; }
    public decimal TotalExpenses { get; set; }
    public decimal RemainingBudget => TotalBudget - TotalExpenses;
    public decimal BudgetUtilizationPercentage => TotalBudget > 0 ? (TotalExpenses / TotalBudget) * 100 : 0;
    public decimal DisbursementRate => TotalBudget > 0 ? (TotalDisbursed / TotalBudget) * 100 : 0;
}

/// <summary>
/// Project profitability analysis
/// </summary>
public class ProjectProfitabilityAnalysis
{
    public Project Project { get; set; } = null!;
    public decimal TotalRevenue { get; set; }
    public decimal TotalCosts { get; set; }
    public decimal GrossProfit => TotalRevenue - TotalCosts;
    public decimal ProfitMargin => TotalRevenue > 0 ? (GrossProfit / TotalRevenue) * 100 : 0;
    public decimal ReturnOnInvestment => TotalCosts > 0 ? (GrossProfit / TotalCosts) * 100 : 0;
}

/// <summary>
/// Project budget utilization
/// </summary>
public class ProjectBudgetUtilization
{
    public Project Project { get; set; } = null!;
    public Dictionary<string, decimal> BudgetByCategory { get; set; } = new();
    public Dictionary<string, decimal> ActualByCategory { get; set; } = new();
    public Dictionary<string, decimal> VarianceByCategory { get; set; } = new();
    public decimal TotalBudget { get; set; }
    public decimal TotalActual { get; set; }
    public decimal TotalVariance => TotalBudget - TotalActual;
}

/// <summary>
/// Project status report
/// </summary>
public class ProjectStatusReport
{
    public Project Project { get; set; } = null!;
    public DateTime ReportDate { get; set; }
    public decimal ProgressPercentage { get; set; }
    public int CompletedMilestones { get; set; }
    public int TotalMilestones { get; set; }
    public int OverdueMilestones { get; set; }
    public ProjectFinancialSummary FinancialSummary { get; set; } = null!;
    public List<string> KeyAchievements { get; set; } = new();
    public List<string> Challenges { get; set; } = new();
    public List<string> NextSteps { get; set; } = new();
}
