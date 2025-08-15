using Microsoft.Extensions.Logging;
using INES.ERP.Core.Interfaces.Repositories;
using INES.ERP.Core.Interfaces.Services;
using INES.ERP.Core.Models.ProjectAccounts;
using INES.ERP.Core.Models.Common;
using INES.ERP.Core.Enums;

namespace INES.ERP.Services.ProjectAccounts;

/// <summary>
/// Project accounts service implementation
/// </summary>
public class ProjectAccountsService : IProjectAccountsService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ProjectAccountsService> _logger;

    public ProjectAccountsService(IUnitOfWork unitOfWork, ILogger<ProjectAccountsService> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    #region Project Management

    public async Task<Project> CreateProjectAsync(Project project, CancellationToken cancellationToken = default)
    {
        try
        {
            // Validate project code uniqueness
            var existingProject = await _unitOfWork.Projects.GetSingleAsync(
                p => p.ProjectCode == project.ProjectCode, cancellationToken);
            
            if (existingProject != null)
            {
                throw new ArgumentException($"Project code '{project.ProjectCode}' already exists.");
            }

            project.CreatedAt = DateTime.UtcNow;
            var createdProject = await _unitOfWork.Projects.AddAsync(project, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Project created successfully: {ProjectCode}", project.ProjectCode);
            return createdProject;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating project: {ProjectCode}", project.ProjectCode);
            throw;
        }
    }

    public async Task<Project> UpdateProjectAsync(Project project, CancellationToken cancellationToken = default)
    {
        try
        {
            project.UpdatedAt = DateTime.UtcNow;
            var updatedProject = await _unitOfWork.Projects.UpdateAsync(project, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Project updated successfully: {ProjectId}", project.Id);
            return updatedProject;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating project: {ProjectId}", project.Id);
            throw;
        }
    }

    public async Task<Project?> GetProjectByIdAsync(Guid projectId, CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.Projects.GetByIdAsync(projectId, cancellationToken);
    }

    public async Task<Project?> GetProjectByCodeAsync(string projectCode, CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.Projects.GetSingleAsync(p => p.ProjectCode == projectCode, cancellationToken);
    }

    public async Task<PagedResult<Project>> GetProjectsAsync(
        int pageNumber = 1,
        int pageSize = 50,
        string? searchTerm = null,
        string? projectType = null,
        Status? status = null,
        string? fundingSource = null,
        CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.Projects.GetPagedAsync(
            pageNumber,
            pageSize,
            predicate: p => 
                (string.IsNullOrEmpty(searchTerm) || 
                 p.ProjectCode.Contains(searchTerm) || 
                 p.Title.Contains(searchTerm) || 
                 p.PrincipalInvestigator.Contains(searchTerm)) &&
                (string.IsNullOrEmpty(projectType) || p.ProjectType == projectType) &&
                (!status.HasValue || p.ProjectStatus == status.Value) &&
                (string.IsNullOrEmpty(fundingSource) || p.FundingSource.Contains(fundingSource)),
            orderBy: q => q.OrderBy(p => p.ProjectCode),
            cancellationToken: cancellationToken);
    }

    #endregion

    #region Milestone Management

    public async Task<ProjectMilestone> CreateMilestoneAsync(ProjectMilestone milestone, CancellationToken cancellationToken = default)
    {
        try
        {
            milestone.CreatedAt = DateTime.UtcNow;
            var createdMilestone = await _unitOfWork.ProjectMilestones.AddAsync(milestone, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Milestone created for project: {ProjectId}", milestone.ProjectId);
            return createdMilestone;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating milestone for project: {ProjectId}", milestone.ProjectId);
            throw;
        }
    }

    public async Task<ProjectMilestone> UpdateMilestoneAsync(ProjectMilestone milestone, CancellationToken cancellationToken = default)
    {
        try
        {
            milestone.UpdatedAt = DateTime.UtcNow;
            var updatedMilestone = await _unitOfWork.ProjectMilestones.UpdateAsync(milestone, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Milestone updated: {MilestoneId}", milestone.Id);
            return updatedMilestone;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating milestone: {MilestoneId}", milestone.Id);
            throw;
        }
    }

    public async Task<IEnumerable<ProjectMilestone>> GetProjectMilestonesAsync(Guid projectId, CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.ProjectMilestones.GetAsync(
            m => m.ProjectId == projectId,
            orderBy: q => q.OrderBy(m => m.MilestoneNumber),
            cancellationToken: cancellationToken);
    }

    public async Task<bool> CompleteMilestoneAsync(Guid milestoneId, DateTime completionDate, CancellationToken cancellationToken = default)
    {
        try
        {
            var milestone = await _unitOfWork.ProjectMilestones.GetByIdAsync(milestoneId, cancellationToken);
            if (milestone == null) return false;

            milestone.ActualEndDate = completionDate;
            milestone.MilestoneStatus = Status.Completed;
            milestone.CompletionPercentage = 100;
            milestone.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.ProjectMilestones.UpdateAsync(milestone, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Milestone completed: {MilestoneId}", milestoneId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error completing milestone: {MilestoneId}", milestoneId);
            return false;
        }
    }

    #endregion

    #region Disbursement Management

    public async Task<ProjectDisbursement> CreateDisbursementAsync(ProjectDisbursement disbursement, CancellationToken cancellationToken = default)
    {
        try
        {
            disbursement.CreatedAt = DateTime.UtcNow;
            var createdDisbursement = await _unitOfWork.ProjectDisbursements.AddAsync(disbursement, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Disbursement created for project: {ProjectId}, Amount: {Amount}", 
                disbursement.ProjectId, disbursement.Amount);
            return createdDisbursement;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating disbursement for project: {ProjectId}", disbursement.ProjectId);
            throw;
        }
    }

    public async Task<bool> ApproveDisbursementAsync(Guid disbursementId, string approvedBy, CancellationToken cancellationToken = default)
    {
        try
        {
            var disbursement = await _unitOfWork.ProjectDisbursements.GetByIdAsync(disbursementId, cancellationToken);
            if (disbursement == null) return false;

            disbursement.DisbursementStatus = Status.Approved;
            disbursement.ApprovedBy = approvedBy;
            disbursement.ApprovalDate = DateTime.UtcNow;
            disbursement.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.ProjectDisbursements.UpdateAsync(disbursement, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Disbursement approved: {DisbursementId} by {ApprovedBy}", disbursementId, approvedBy);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error approving disbursement: {DisbursementId}", disbursementId);
            return false;
        }
    }

    public async Task<IEnumerable<ProjectDisbursement>> GetProjectDisbursementsAsync(Guid projectId, CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.ProjectDisbursements.GetAsync(
            d => d.ProjectId == projectId,
            orderBy: q => q.OrderByDescending(d => d.DisbursementDate),
            cancellationToken: cancellationToken);
    }

    #endregion

    #region Expense Management

    public async Task<ProjectExpense> RecordExpenseAsync(ProjectExpense expense, CancellationToken cancellationToken = default)
    {
        try
        {
            expense.CreatedAt = DateTime.UtcNow;
            var recordedExpense = await _unitOfWork.ProjectExpenses.AddAsync(expense, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Expense recorded for project: {ProjectId}, Amount: {Amount}", 
                expense.ProjectId, expense.Amount);
            return recordedExpense;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error recording expense for project: {ProjectId}", expense.ProjectId);
            throw;
        }
    }

    public async Task<IEnumerable<ProjectExpense>> GetProjectExpensesAsync(
        Guid projectId,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        string? category = null,
        CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.ProjectExpenses.GetAsync(
            e => e.ProjectId == projectId &&
                 (!fromDate.HasValue || e.ExpenseDate >= fromDate.Value) &&
                 (!toDate.HasValue || e.ExpenseDate <= toDate.Value) &&
                 (string.IsNullOrEmpty(category) || e.Category == category),
            orderBy: q => q.OrderByDescending(e => e.ExpenseDate),
            cancellationToken: cancellationToken);
    }

    #endregion

    #region Financial Analysis

    public async Task<ProjectFinancialSummary> GetProjectFinancialSummaryAsync(Guid projectId, CancellationToken cancellationToken = default)
    {
        try
        {
            var project = await _unitOfWork.Projects.GetByIdAsync(projectId, cancellationToken);
            if (project == null)
                throw new ArgumentException($"Project not found: {projectId}");

            var disbursements = await GetProjectDisbursementsAsync(projectId, cancellationToken);
            var expenses = await GetProjectExpensesAsync(projectId, cancellationToken: cancellationToken);

            var totalDisbursed = disbursements
                .Where(d => d.DisbursementStatus == Status.Completed)
                .Sum(d => d.Amount);

            var totalExpenses = expenses
                .Where(e => e.ExpenseStatus == Status.Completed)
                .Sum(e => e.Amount);

            return new ProjectFinancialSummary
            {
                Project = project,
                TotalBudget = project.TotalBudget,
                TotalDisbursed = totalDisbursed,
                TotalExpenses = totalExpenses
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting financial summary for project: {ProjectId}", projectId);
            throw;
        }
    }

    public async Task<ProjectProfitabilityAnalysis> GetProjectProfitabilityAsync(Guid projectId, CancellationToken cancellationToken = default)
    {
        try
        {
            var project = await _unitOfWork.Projects.GetByIdAsync(projectId, cancellationToken);
            if (project == null)
                throw new ArgumentException($"Project not found: {projectId}");

            var expenses = await GetProjectExpensesAsync(projectId, cancellationToken: cancellationToken);
            var totalCosts = expenses.Where(e => e.ExpenseStatus == Status.Completed).Sum(e => e.Amount);

            return new ProjectProfitabilityAnalysis
            {
                Project = project,
                TotalRevenue = project.TotalBudget, // Assuming budget is revenue for grants
                TotalCosts = totalCosts
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting profitability analysis for project: {ProjectId}", projectId);
            throw;
        }
    }

    public async Task<ProjectBudgetUtilization> GetBudgetUtilizationAsync(Guid projectId, CancellationToken cancellationToken = default)
    {
        try
        {
            var project = await _unitOfWork.Projects.GetByIdAsync(projectId, cancellationToken);
            if (project == null)
                throw new ArgumentException($"Project not found: {projectId}");

            var expenses = await GetProjectExpensesAsync(projectId, cancellationToken: cancellationToken);
            var completedExpenses = expenses.Where(e => e.ExpenseStatus == Status.Completed);

            var actualByCategory = completedExpenses
                .GroupBy(e => e.Category)
                .ToDictionary(g => g.Key, g => g.Sum(e => e.Amount));

            // For simplicity, assume equal budget allocation across categories
            var categories = actualByCategory.Keys.ToList();
            var budgetPerCategory = categories.Count > 0 ? project.TotalBudget / categories.Count : 0;
            var budgetByCategory = categories.ToDictionary(c => c, c => budgetPerCategory);

            var varianceByCategory = budgetByCategory.ToDictionary(
                kvp => kvp.Key, 
                kvp => kvp.Value - actualByCategory.GetValueOrDefault(kvp.Key, 0));

            return new ProjectBudgetUtilization
            {
                Project = project,
                BudgetByCategory = budgetByCategory,
                ActualByCategory = actualByCategory,
                VarianceByCategory = varianceByCategory,
                TotalBudget = project.TotalBudget,
                TotalActual = actualByCategory.Values.Sum()
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting budget utilization for project: {ProjectId}", projectId);
            throw;
        }
    }

    #endregion

    #region Reporting

    public async Task<ProjectStatusReport> GenerateStatusReportAsync(Guid projectId, DateTime reportDate, CancellationToken cancellationToken = default)
    {
        try
        {
            var project = await _unitOfWork.Projects.GetByIdAsync(projectId, cancellationToken);
            if (project == null)
                throw new ArgumentException($"Project not found: {projectId}");

            var milestones = await GetProjectMilestonesAsync(projectId, cancellationToken);
            var financialSummary = await GetProjectFinancialSummaryAsync(projectId, cancellationToken);

            var totalMilestones = milestones.Count();
            var completedMilestones = milestones.Count(m => m.MilestoneStatus == Status.Completed);
            var overdueMilestones = milestones.Count(m => m.IsOverdue);

            return new ProjectStatusReport
            {
                Project = project,
                ReportDate = reportDate,
                ProgressPercentage = project.ProgressPercentage,
                CompletedMilestones = completedMilestones,
                TotalMilestones = totalMilestones,
                OverdueMilestones = overdueMilestones,
                FinancialSummary = financialSummary,
                KeyAchievements = new List<string> { "Sample achievement 1", "Sample achievement 2" },
                Challenges = new List<string> { "Sample challenge 1", "Sample challenge 2" },
                NextSteps = new List<string> { "Sample next step 1", "Sample next step 2" }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating status report for project: {ProjectId}", projectId);
            throw;
        }
    }

    public async Task<IEnumerable<Project>> GetOverdueProjectsAsync(CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.Projects.GetAsync(
            p => p.EndDate < DateTime.Today && p.ProjectStatus != Status.Completed,
            orderBy: q => q.OrderBy(p => p.EndDate),
            cancellationToken: cancellationToken);
    }

    #endregion
}
