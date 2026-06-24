using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TraineeManagement.Api.Data.ModelTimestampInterface;
using TraineeManagement.Api.Data.SubmissionModel;

namespace TraineeManagement.Api.Data.SubmissionFileModel;

    public class SubmissionFile : ICreateTimestamp
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id 
        { 
            get; 
            set; 
        }

        public int SubmissionId 
        { 
            get; 
            set; 
        }
         public Submission? Submission 
        { 
            get; 
            set; 
        }

        public string OriginalFileName 
        { 
            get; 
            set; 
        } = string.Empty;
        
        public string StorageFileName 
        { 
            get; 
            set; 
        } = string.Empty;

        public string ContentType 
        { 
            get; 
            set; 
        } = string.Empty;

        public long Size 
        { 
            get; 
            set; 
        }

        public string Checksum 
        { 
            get; 
            set; 
        } = string.Empty;

        public string UploadedByUserId 
        { 
            get; 
            set; 
        } = string.Empty;

        public DateTime CreatedDate
        { 
            get; 
            set; 
        }
    }
