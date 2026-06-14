using Microsoft.EntityFrameworkCore;
using TraineeManagementApi.Models.TimestampInterface;
using TraineeManagementApi.Trainees.Models;
using TraineeManagementApi.Users.Models;
using TraineeManagementApi.Mentors.Models;
using TraineeManagementApi.LearningTasks.Models;
using TraineeManagementApi.TaskAssignments.Models;
using TraineeManagementApi.Submissions.Models;
using TraineeManagementApi.Reviews.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Trainee> Trainees { get; set; }
    
    public DbSet<User> Users { get; set; }

    public DbSet<Mentor> Mentors { get; set; }

    public DbSet<LearningTask> LearningTasks { get; set; }

    public DbSet<TaskAssignment> TaskAssignments { get; set; }

    public DbSet<Submission> Submissions { get; set; }

    public DbSet<Submission> Reviews { get; set; }

    public override int SaveChanges()
    {
        ApplyTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void ApplyTimestamps()
    {
        var entries = ChangeTracker
            .Entries<ITimestamp>()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedDate = DateTime.UtcNow;
            }
            entry.Entity.UpdatedDate = DateTime.UtcNow;
        }

        var assignDate = ChangeTracker
            .Entries<TaskAssignmentTimeStamp>()
            .Where(e => e.State == EntityState.Added);

        foreach (var entry in assignDate)
        {
            entry.Entity.AssignedDate = DateOnly.FromDateTime(DateTime.UtcNow);
        }

        var submittedDate = ChangeTracker
            .Entries<SubmissionTimeStamp>()
            .Where(e => e.State == EntityState.Added);

        foreach (var entry in submittedDate)
        {
            entry.Entity.SubmittedDate = DateOnly.FromDateTime(DateTime.UtcNow);
        }

        var reviewedDate = ChangeTracker
            .Entries<ReviewTimeStamp>()
            .Where(e => e.State == EntityState.Added);

        foreach (var entry in reviewedDate)
        {
            entry.Entity.ReviewedDate = DateOnly.FromDateTime(DateTime.UtcNow);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Trainee>()
            .Property(t => t.Status)
            .HasConversion<string>();

        modelBuilder.Entity<User>()
            .Property(u => u.Role)
            .HasConversion<string>();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

        modelBuilder.Entity<Mentor>()
            .Property(m => m.Status)
            .HasConversion<string>();

        modelBuilder.Entity<LearningTask>()
            .Property(lt => lt.Status)
            .HasConversion<string>();

        modelBuilder.Entity<TaskAssignment>()
            .Property(ta => ta.Status)
            .HasConversion<string>();

        modelBuilder.Entity<Submission>()
            .Property(s => s.Status)
            .HasConversion<string>();

        modelBuilder.Entity<Review>()
            .Property(r => r.Status)
            .HasConversion<string>();

        modelBuilder.Entity<TaskAssignment>()
            .HasOne(tr => tr.Trainee)                 
            .WithMany(t => t.TaskAssignments)         
            .HasForeignKey(tr => tr.TraineeId)       
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TaskAssignment>()
            .HasOne(me => me.Mentor)                 
            .WithMany(m => m.TaskAssignments)         
            .HasForeignKey(me => me.MentorId)       
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<TaskAssignment>()
            .HasOne(lt => lt.LearningTask)                 
            .WithMany(l => l.TaskAssignments)         
            .HasForeignKey(lt => lt.LearningTaskId)       
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Submission>()
            .HasOne(lt => lt.TaskAssignment)                 
            .WithMany(l => l.Submissions)         
            .HasForeignKey(lt => lt.TaskAssignmentId)       
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Review>()
            .HasOne(lt => lt.Submission)                 
            .WithMany(l => l.Reviews)         
            .HasForeignKey(lt => lt.SubmissionId)       
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Review>()
            .HasOne(lt => lt.Mentor)                 
            .WithMany(l => l.Reviews)         
            .HasForeignKey(lt => lt.MentorId)       
            .OnDelete(DeleteBehavior.Restrict);
        
    }
}