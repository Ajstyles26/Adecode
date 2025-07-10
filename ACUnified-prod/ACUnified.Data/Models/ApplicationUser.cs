using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using ACUnified.Data.Enum;


namespace ACUnified.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public UserType CurrentUserType { get; set; }
        public long UWPId { get; set; }
        public ApplicationStage CurrentApplicationStage { get; set; } = ApplicationStage.Stage1;
        public string ACUNo { get; set; } = "";
        [Column(TypeName = "varchar(50)")]
        public string MatricNo { get; set; } = "";
        [Column(TypeName = "varchar(50)")]
        public string Firstname { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string Surname { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string? Othername { get; set; } = "";
        [Column(TypeName = "varchar(50)")]
        public string? Session { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string ProfilePicture { get; set; } = "/upload/blank-person.png";
        //institution provider
        public virtual long? InstitutionProviderId { get; set; }
        public virtual InstitutionProvider? InstitutionProvider { get; set; }
        public AuthStatus AuthenticationStatus { get; set; } = AuthStatus.DefaultPW;
    }
}

