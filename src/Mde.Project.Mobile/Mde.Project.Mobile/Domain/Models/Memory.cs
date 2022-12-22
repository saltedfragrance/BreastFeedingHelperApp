using Mde.Project.Mobile.Domain.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.CommunityToolkit.Core;
using Xamarin.Forms;

namespace Mde.Project.Mobile.Domain.Models
{
    public class Memory : BaseModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public Guid MotherId { get; set; }
        public Mother Mother { get; set; }
        public Guid BabyId { get; set; }
        public Baby Baby { get; set; }
        public string MemoryUrl { get; set; }
        public ImageSource MemoryImage { get; set; }
        public MediaSource MemoryVideo { get; set; }
        public bool IsPicture { get; set; }
        public bool IsMovie { get; set; }
        public int ImageRotation { get; set; }
        public string FileName { get; set; }
    }
}
