using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserValidatorLib
{
    public class UserValidator
    {
        UserNameEncrypter encrypter;

        public UserValidator()
        {
            encrypter = new UserNameEncrypter();
        }

        public bool validate(String userName, String encryptedString)
        {
            String encryptedUserName = encrypter.encryptFromUserName(userName);
            return encryptedUserName.Equals(encryptedString);
        }
    }
}
