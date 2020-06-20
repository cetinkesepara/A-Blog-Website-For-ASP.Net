using System;
using System.Collections.Generic;
using System.Text;

namespace BlogWeb.Entity
{
    public class Setting
    {
        public int SettingId { get; set; }
        public string SiteName { get; set; }
        public string SiteTitle { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }

        public string SMTPServerHost { get; set; }
        public string SMTPServerUsername { get; set; }
        public string SMTPServerPassword { get; set; }
        public string SMTPServerPort { get; set; }
        public string SMTPServerFrom { get; set; }
        public string SMTPServerFromName { get; set; }

        public string AboutPage { get; set; }
        public string ContactPage { get; set; }
        public string FrequentlyAskedPage { get; set; }
        public string PrivacyPolicyPage { get; set; }

        public string Facebook { get; set; }
        public string Instagram { get; set; }
        public string Twitter { get; set; }
        public string Linkedin { get; set; }
        public string Youtube { get; set; }

        public string FacebookSharing { get; set; }
        public string TwitterSharing { get; set; }
        public string LinkedInSharing { get; set; }

        public int TakeBlogCount { get; set; }
        public int TakeCommentCount { get; set; }
        public int TakeAnswerCount { get; set; }
        public int TakeSideMostReadCount { get; set; }
        public int TakeSideMostCommentCount { get; set; }
        public int TakeSideLastPublishedCount { get; set; }
        public int TakeSideRandomBlogCount { get; set; }
    }
}
