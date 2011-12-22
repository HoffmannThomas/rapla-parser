using System;

namespace WebService
{
    class User
    {
        public String rapla_user_id { private set; get; }
        private String exchange_user_name;
        private String password;

        public User(String rapla_user_id, String exchange_user_name, String plain_password, Boolean encrypt)
        {
            this.rapla_user_id = rapla_user_id;
            this.exchange_user_name = exchange_user_name;

            if (encrypt)
            {
                this.exchange_user_name = Encryption.EncryptStringAES(exchange_user_name, this.rapla_user_id);
                this.password = Encryption.EncryptStringAES(plain_password, this.rapla_user_id + this.exchange_user_name);
            }
            else
            {
                this.exchange_user_name = exchange_user_name;
                this.password = plain_password;
            }
        }

        public String getPassword()
        {
            return Encryption.DecryptStringAES(this.password, this.rapla_user_id + this.exchange_user_name);
        }
        public String getExchangeUserName()
        {
            return Encryption.DecryptStringAES(this.exchange_user_name, this.rapla_user_id);
        }

        public override string ToString()
        {
            return this.rapla_user_id + ";" + this.exchange_user_name + ";" + this.password;
        }


    }
}
