using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [FriendOf(typeof (DlgAccountLogin))]
    public static class DlgAccountLoginSystem
    {
        public static void RegisterUIEvent(this DlgAccountLogin self)
        {
            self.View.E_LoginButton.AddListenerAsync(self.OnLoginButtonClickHandler);
        }

        public static void ShowWindow(this DlgAccountLogin self, Entity contextData = null)
        {
            string[] lines = File.ReadAllLines(@"..\Config\ServerAddress.txt");
            self.View.E_ServerAddressDropdown.options.Clear();
            foreach (string address in lines)
            {
                Dropdown.OptionData optionData = new();
                optionData.text = address;
                self.View.E_ServerAddressDropdown.options.Add(optionData);
            }
            self.View.E_AccountTMP_InputField.text = PlayerPrefs.GetString("Account", string.Empty);
            self.View.E_PasswordTMP_InputField.text = PlayerPrefs.GetString("Password", string.Empty);
        }

        private static async ETTask OnLoginButtonClickHandler(this DlgAccountLogin self)
        {
            var account = self.View.E_AccountTMP_InputField.text;
            var password = self.View.E_PasswordTMP_InputField.text;
            var address = self.View.E_ServerAddressDropdown.options[self.View.E_ServerAddressDropdown.value].text;
            await LoginHelper.Login(self.Root(), account, password, address);

            PlayerPrefs.SetString("Account", account);
            PlayerPrefs.SetString("Password", password);
        }
    }
}