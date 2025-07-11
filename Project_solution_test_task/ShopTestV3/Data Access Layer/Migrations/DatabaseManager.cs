﻿using Core;
using Core.Interfaces.Models;
using LayerDataAccess.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LayerDataAccess.Repositories;

namespace LayerDataAccess.Migrations
{
	public static class DatabaseManager
	{
		private static AppDbContext? сontext = null;
		public static AppDbContext Сontext
		{
			get
			{
				while (сontext == null) InitializeDatabase(Config.strDatabaseOptions);
				return сontext;
			}
		}

		public static void InitializeDatabase(string strOptions)
		{
			Massage.Log("strOptions: " + strOptions);

			DbContextOptionsBuilder<AppDbContext> options = new();
			options.UseNpgsql(strOptions);
			сontext = new(options.Options);

			try
			{
				if (!Сontext.Database.CanConnect())
				{
					Massage.Log("Creating database and tables... ");

					Сontext.Database.EnsureCreated();

					Massage.Log("Database created successfully!");
				}
				else
				{
					Massage.LogGood("Database already exists.");
				}
			}
			catch (Exception ex)
			{
				Massage.LogError("Error:");
				Massage.Log(ex.Message);
			}

			AddTestUsers();
		}

		private static void AddTestUsers()
		{
			Random ran = new Random((int)DateTime.Now.Ticks);

			GenerationUsers(ran.Next(26, 84), Role.Default, ran);
			GenerationUsers(ran.Next(4, 16), Role.Admin, ran);

			Сontext?.SaveChanges();
		}

		//warning: cringe code
		private static void GenerationUsers(int countUser, Role role, Random ran)
		{
			string[] syllables = new string[] {"ing"
,"er"
,"a"
,"ly"
,"ed"
,"i"
,"es"
,"re"
,"tion"
,"in"
,"e"
,"con"
,"y"
,"ter"
,"ex"
,"al"
,"de"
,"com"
,"o"
,"di"
,"en"
,"an"
,"ty"
,"ry"
,"u"
,"ti"
,"ri"
,"be"
,"per"
,"to"
,"pro"
,"ac"
,"ad"
,"ar"
,"ers"
,"ment"
,"or"
,"tions"
,"ble"
,"der"
,"ma"
,"na"
,"si"
,"un"
,"at"
,"dis"
,"ca"
,"cal"
,"man"
,"ap"
,"po"
,"sion"
,"vi"
,"el"
,"est"
,"la"
,"lar"
,"pa"
,"ture"
,"for"
,"is"
,"mer"
,"pe"
,"ra"
,"so"
,"ta"
,"as"
,"col"
,"fi"
,"ful"
,"ger"
,"low"
,"ni"
,"par"
,"son"
,"tle"
,"day"
,"ny"
,"pen"
,"pre"
,"tive"
,"car"
,"ci"
,"mo"
,"on"
,"ous"
,"pi"
,"se"
,"ten"
,"tor"
,"ver"
,"ber"
,"can"
,"dy"
,"et"
,"it"
,"mu"
,"no"
,"ple"
,"cu"
,"fac"
,"fer"
,"gen"
,"ic"
,"land"
,"light"
,"ob"
,"of"
,"pos"
,"tain"
,"den"
,"ings"
,"mag"
,"ments"
,"set"
,"some"
,"sub"
,"sur"
,"ters"
,"tu"
,"af"
,"au"
,"cy"
,"fa"
,"im"
,"li"
,"lo"
,"men"
,"min"
,"mon"
,"op"
,"out"
,"rec"
,"ro"
,"sen"
,"side"
,"tal"
,"tic"
,"ties"
,"ward"
,"age"
,"ba"
,"but"
,"cit"
,"cle"
,"co"
,"cov"
,"da"
,"dif"
,"ence"
,"ern"
,"eve"
,"hap"
,"ies"
,"ket"
,"lec"
,"main"
,"mar"
,"mis"
,"my"
,"nal"
,"ness"
,"ning"
,"n't"
,"nu"
,"oc"
,"pres"
,"sup"
,"te"
,"ted"
,"tem"
,"tin"
,"tri"
,"tro"
,"up"
,"va"
,"ven"
,"vis"
,"am"
,"bor"
,"by"
,"cat"
,"cent"
,"ev"
,"gan"
,"gle"
,"head"
,"high"
,"il"
,"lu"
,"me"
,"nore"
,"part"
,"por"
,"read"
,"rep"
,"su"
,"tend"
,"ther"
,"ton"
,"try"
,"um"
,"uer"
,"way"
,"ate"
,"bet"
,"bles"
,"bod"
,"cap"
,"cial"
,"cir"
,"cor"
,"coun"
,"cus"
,"dan"
,"dle"
,"ef"
,"end"
,"ent"
,"ered"
,"fin"
,"form"
,"go"
,"har"
,"ish"
,"lands"
,"let"
,"long"
,"mat"
,"meas"
,"mem"
,"mul"
,"ner"
,"play"
,"ples"
,"ply"
,"port"
,"press"
,"sat"
,"sec"
,"ser"
,"south"
,"sun"
,"the"
,"ting"
,"tra"
,"tures"
,"val"
,"var"
,"vid"
,"wil"
,"win"
,"won"
,"work"
,"act"
,"ag"
,"air"
,"als"
,"bat"
,"bi"
,"cate"
,"cen"
,"char"
,"come"
,"cul"
,"ders"
,"east"
,"fect"
,"fish"
,"fix"
,"gi"
,"grand"
,"great"
,"heav"
,"ho"
,"hunt"
,"ion"
,"its"
,"jo"
,"lat"
,"lead"
,"lect"
,"lent"
,"less"
,"lin"
,"mal"
,"mi"
,"mil"
,"moth"
,"near"
,"nel"
,"net"
,"new"
,"one"
,"point"
,"prac"
,"ral"
,"rect"
,"ried"
,"round"
,"row"
,"sa"
,"sand"
,"self"
,"sent"
,"ship"
,"sim"
,"sions"
,"sis"
,"sons"
,"stand"
,"sug"
,"tel"
,"tom"
,"tors"
,"tract"
,"tray"
,"us"
,"vel"
,"west"
,"where"
,"writ"
				};

			string[] paswordTokens = new string[]
				{
					"!",
					"@",
					"$",
					"%",
					"^",
					"&",
					"*",
					"(",
					")",
					"-",
					"+",
					"=",
					"_",
					"{",
					"}",
					"<",
					">",
					":",
					";",
					"~",
					"1",
					"2",
					"3",
					"4",
					"5",
					"6",
					"7",
					"8",
					"9",
					"0"
				};

			int
				lengthEmail,
				lengthNums;

			List<string> listEmail = new();
			string
				emailByffer = string.Empty,
					passwordBuffer = string.Empty;

			for (int i = 0; i < countUser; i++)
			{
				lengthEmail = ran.Next(2, 6);

				for (int j = 0; j < lengthEmail; j++)
				{
					emailByffer += syllables[ran.Next(0, syllables.Length)];

					if (ran.Next(0, 6) == 6)
					{
						lengthNums = ran.Next(1, 6);

						for (int k = 0; k < lengthNums; k++)
						{
							emailByffer += ran.Next(0, 9);
						}
					}
				}

				emailByffer += "@";

				lengthEmail = ran.Next(1, 3);

				for (int j = 0; j < lengthEmail; j++)
				{
					emailByffer += syllables[ran.Next(0, syllables.Length)];
				}

				emailByffer += "." + syllables[ran.Next(0, syllables.Length)];

				listEmail.Add(emailByffer);
				emailByffer = string.Empty;
			}

			foreach (string email in listEmail)
			{
				lengthNums = ran.Next(4, 16);
				for (int i = 0; i < lengthNums; i++)
				{
					passwordBuffer += paswordTokens[ran.Next(0, paswordTokens.Length)];
				}

				if (сontext != null)
				{
					string hash = RepositoryUser.PasswordHash(passwordBuffer, email, сontext.Users.Max(u => u.Id));

					Console.WriteLine(email);
					Console.WriteLine(role.ToString());
					Console.WriteLine(hash);

					User user = new User
					{
						Email = email,
						Role = role,
						Password = hash,
						Money = ran.Next(0, 9999)
					};



					Сontext?.Users.Add(user);

					Console.WriteLine($"email: {user.Email},{new string(' ', (int)Math.Clamp(34 - user.Email.Length, 4, 34))}password: {passwordBuffer}");

					passwordBuffer = string.Empty;

				}
			}
		}
	}
}
