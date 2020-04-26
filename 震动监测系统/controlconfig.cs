using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace �𶯼��ϵͳ
{
    static class controlconfig
    {
        //��Ӽ�ΪkeyName��ֵΪkeyValue���
        public static void addItem(string keyName, string keyValue)
        {
            //��������ļ������ΪkeyName��ֵΪkeyValue
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Add(keyName, keyValue);
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        //�жϼ�ΪkeyName�����Ƿ���ڣ�
        public static bool existItem(string keyName)
        {
            //�ж������ļ����Ƿ���ڼ�ΪkeyName����
            foreach (string key in ConfigurationManager.AppSettings)
            {
                if (key == keyName)
                {
                    //����
                    return true;
                }
            }
            return false;
        }

        //��ȡ��ΪkeyName�����ֵ��
        public static string valueItem(string keyName)
        {
            //���������ļ��м�ΪkeyName�����ֵ
            return ConfigurationManager.AppSettings[keyName];
        }

        //�޸ļ�ΪkeyName�����ֵ��
        public static void modifyItem(string keyName, string newKeyValue)
        {
            //�޸������ļ��м�ΪkeyName�����ֵ
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings[keyName].Value = newKeyValue;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        //ɾ����ΪkeyName���
        public static void removeItem(string keyName)
        {
            //ɾ�������ļ���ΪkeyName����
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Remove(keyName);
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        /// <summary>
        /// �Զ��ж��Ƿ����Item
        /// �����ڣ��޸�ֵ
        /// �����ڣ����ֵ
        /// </summary>
        /// <param name="keyName"></param>
        public static void AddOrModifyItem(string keyName, string newKeyValue)
        {
            if (existItem(keyName))
            {
                modifyItem(keyName, newKeyValue);
            }
            else
            {
                addItem(keyName, newKeyValue);
            }
        }
    }
}
