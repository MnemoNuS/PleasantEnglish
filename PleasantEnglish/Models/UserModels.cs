using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace PleasantEnglish.Models
{

	public class User : IdentityUser
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int UserId { get; set; }
		public string Name { get; set; }
		public string UserImg { get; set; }
		public virtual ICollection<Like> UserLikes { get; set; }
		public virtual ICollection<Watch> UserWatches { get; set; }
		public virtual ICollection<Comment> UserComments { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
		{
			// Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
			var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
			// Add custom user claims here
			return userIdentity;
		}

		public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager, bool isPersistent)
		{
			var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
			userIdentity.SetIsPersistent(isPersistent);
			return userIdentity;
		}
	}

	public static class ClaimsIdentityExtensions
	{
		private const string PersistentLoginClaimType = "PersistentLogin";

		public static bool GetIsPersistent(this System.Security.Claims.ClaimsIdentity identity)
		{
			return identity.Claims.FirstOrDefault(c => c.Type == PersistentLoginClaimType) != null;
		}

		public static void SetIsPersistent(this System.Security.Claims.ClaimsIdentity identity, bool isPersistent)
		{
			var claim = identity.Claims.FirstOrDefault(c => c.Type == PersistentLoginClaimType);
			if (isPersistent)
			{
				if (claim == null)
				{
					identity.AddClaim(new System.Security.Claims.Claim(PersistentLoginClaimType, Boolean.TrueString));
				}
			}
			else if (claim != null)
			{
				identity.RemoveClaim(claim);
			}
		}
	}

	public class Pupil
	{
	    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	    [Key, Column(Order = 0)]
        public int PupilId { get; set; }
	    [Key, Column(Order = 1)]
        public virtual int UserId { get; set; }
        public virtual User User { get; set; }
        public bool IsActive { get; set; }
		public virtual ICollection<PupilCollection> Collections { get; set; }
		public virtual ICollection<PupilSchoolwork> Schoolworks { get; set; }
	    public virtual ICollection<Invite> Invites { get; set; }

    }

    public class PupilCollection
	{
		[Key, Column(Order = 0)]
		public int PupilId { get; set; }
		[Key, Column(Order = 1)]
		public int CollectionId { get; set; }
		public Pupil Pupil { get; set; }
		public Collection Collection { get; set; }
	}

    public class Schoolwork
	{
		public int SchoolworkId { get; set; }
		public DateTime? Date { get; set; }
		public DateTime? TimeStart { get; set; }
		public DateTime? TimeEnd { get; set; }
		public int Duration { get; set; }
		public string HomeTask { get; set; }
		public virtual ICollection<PupilSchoolwork> PupilSchoolworks { get; set; }

		public Schoolwork()
		{
			SchoolworkId = -1;
			Date = DateTime.Now;
			TimeStart = DateTime.Now;
			TimeEnd = DateTime.Now;
			Duration = 0;
			HomeTask = "no";
		}
	}

	public class PupilSchoolwork
	{
		[Key, Column(Order = 0)]
		public int SchoolworkId { get; set; }
		public Schoolwork Schoolwork { get; set; }
		[Key, Column(Order = 1)]
		public int PupilId { get; set; }
		public Pupil Pupil { get; set; }
	}

	public class RoomData
	{
	    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	    public int RoomDataId { get; set; }
	    public string RoomId { get; set; }
	    public bool IsClosed { get; set; }
	    public DateTime? DateCreated { get; set; }
	    public DateTime? DateUpdated { get; set; }
	    public string Data { get; set; }
	    public string ConnectionLinks { get; set; }
	    public virtual ICollection<Invite> Invites { get; set; }
    }

    public class Invite
    {
        [Key, Column(Order = 0)]
        public virtual int RoomDataId { get; set; }
        [Key, Column(Order = 1)]
        public virtual int PupilId { get; set; }
        public virtual Pupil Pupil { get; set; }
        public virtual RoomData RoomData { get; set; }
        public string ConnectionLink { get; set; }
    }
}