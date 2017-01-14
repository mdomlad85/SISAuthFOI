using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using IdentitySample.Models;
using Owin;
using System;
using Web.Properties;

namespace IdentitySample
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            // Konfiguracija konteksta baze, upravitelj korisnicima i rolama kako bi se koristila ista instanca po zahtjevu
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Omogućava aplikaciji spremi podatke o prijavljenom korisniku u cookie
            // Konfiguracija login cookie-a
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Omogućava aplikaciji da validira siguronosni token kad se korisnik prijavi
                    // Koristi se prilikom zamjene lozinke ili dodavanje logina sa vanjskim sustavima (npr. Facebook)
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }                
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Omogućava da aplikacija privremeno spremi korisničke informacije dok se verificira drugi korak i vrijeme njegova čuvanja
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(Settings.Default.CookieDuration));

            // Omogućava da se zapamti drugi dio verifikacije, kao što je email ili mobitel.
            // Jednom kad se odabere ova opcija prilikom prijave na tom računalu i pregledniku proces će biti zapamćen.
            // Slično kao zapamti me opcija pri prvom dijelu prijave.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Provideri za prijavu treće strane

            app.UseFacebookAuthentication(
               appId: Settings.Default.FacebookID,
               appSecret: Settings.Default.FacebookSecret);

            app.UseGoogleAuthentication(
                clientId: Settings.Default.GoogleID,
                clientSecret: Settings.Default.GoogleSecret);
        }
    }
}