using BlogWeb.Data.Abstract;
using BlogWeb.Data.Concrete.EFCore;
using BlogWeb.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlogWeb.Data.Concrete
{
    public class SettingRepository : ISettingRepository
    {
        private readonly DBContext context;
        public SettingRepository(DBContext _context)
        {
            context = _context;
        }

        public Setting GetById(int settingId)
        {
            return context.Settings.First(p => p.SettingId == settingId);
        }

        public bool UpdateSetting(Setting setting)
        {
            try
            {
                var settings = GetById(setting.SettingId);
                if(setting != null)
                {
                    settings.SiteName = setting.SiteName;
                    settings.SiteTitle = setting.SiteTitle;
                    settings.Description = setting.Description;

                    settings.Email = setting.Email;
                    settings.Phone = setting.Phone;
                    settings.Address = setting.Address;
                    settings.City = setting.City;

                    settings.SMTPServerHost = setting.SMTPServerHost;
                    settings.SMTPServerUsername = setting.SMTPServerUsername;
                    settings.SMTPServerPassword = setting.SMTPServerPassword;
                    settings.SMTPServerPort = setting.SMTPServerPort;
                    settings.SMTPServerFrom = setting.SMTPServerFrom;
                    settings.SMTPServerFromName = setting.SMTPServerFromName;

                    settings.AboutPage = setting.AboutPage;
                    settings.ContactPage = setting.ContactPage;
                    settings.FrequentlyAskedPage = setting.FrequentlyAskedPage;
                    settings.PrivacyPolicyPage = setting.PrivacyPolicyPage;

                    settings.Facebook = setting.Facebook;
                    settings.Instagram = setting.Instagram;
                    settings.Twitter = setting.Twitter;
                    settings.Linkedin = setting.Linkedin;
                    settings.Youtube = setting.Youtube;

                    settings.FacebookSharing = setting.FacebookSharing;
                    settings.TwitterSharing = setting.TwitterSharing;
                    settings.LinkedInSharing = setting.LinkedInSharing;

                    settings.TakeBlogCount = setting.TakeBlogCount;
                    settings.TakeCommentCount = setting.TakeCommentCount;
                    settings.TakeAnswerCount = setting.TakeAnswerCount;
                    settings.TakeSideMostReadCount = setting.TakeSideMostReadCount;
                    settings.TakeSideMostCommentCount = setting.TakeSideMostCommentCount;
                    settings.TakeSideLastPublishedCount = setting.TakeSideLastPublishedCount;
                    settings.TakeSideRandomBlogCount = setting.TakeSideRandomBlogCount;

                    context.SaveChanges();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
