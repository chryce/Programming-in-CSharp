﻿using IdentityServer4.Models;
using System.Collections.Generic;

namespace V2ID4.IdServer
{
    public class Config
    {
        /// <summary>
        /// 返回应用列表
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetApiResources()
        {
            List<ApiResource> resources = new List<ApiResource>();
            //ApiResource第一个参数是应用的名字，第二个参数是描述
            resources.Add(new ApiResource("MsgAPI", "消息服务API"));
            resources.Add(new ApiResource("ProductAPI", "产品API"));
            return resources;
        }

        /// <summary>
        /// 返回账号列表
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Client> GetClients()
        {
            List<Client> clients = new List<Client>();
            clients.Add(new Client
            {
                ClientId = "clientPC1",//API账号、客户端Id
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets =
                {
                    new Secret("123321".Sha256())//秘钥
                },
                AllowedScopes = { "MsgAPI", "ProductAPI" }//这个账号支持访问哪些应用
            });
            clients.Add(new Client
            {
                ClientId = "clientAndroid",//API账号、客户端Id
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets =
                {
                    new Secret("a2$44c".Sha256())//秘钥
                },
                AllowedScopes = { "MsgAPI" }//这个账号支持访问哪些应用
            });
            clients.Add(new Client
            {
                ClientId = "clientios",//API账号、客户端Id
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets =
                {
                    new Secret("a23334c".Sha256())//秘钥
                },
                AllowedScopes = { "MsgAPI" }//这个账号支持访问哪些应用
            });
            return clients;
        }
    }
}