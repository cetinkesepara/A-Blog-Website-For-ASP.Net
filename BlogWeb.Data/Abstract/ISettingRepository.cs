using BlogWeb.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogWeb.Data.Abstract
{
    public interface ISettingRepository
    {
        Setting GetById(int settingId);
        bool UpdateSetting(Setting setting);
    }
}
