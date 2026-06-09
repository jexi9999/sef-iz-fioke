using SefIzFioke.Data;
using SefIzFioke.Helpers;
using SefIzFioke.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace SefIzFioke
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            using var db = new AppDbContext();
            db.Database.EnsureCreated();
            SeedData(db);
        }

        private static void SeedData(AppDbContext db)
        {
            if (db.Korisnici.Any()) return;

            // ===== KORISNICI =====
            var admin = new Korisnik { KorisnickoIme = "admin", Ime = "Admin", Prezime = "Administrator", Email = "admin@sefizfioke.rs", Lozinka = PasswordHelper.Hash("admin123"), IsAdmin = true };
            var masa = new Korisnik { KorisnickoIme = "MasaMilic", Ime = "Maša", Prezime = "Milić", Email = "masa@gmail.com", Lozinka = PasswordHelper.Hash("masa123") };
            var danica = new Korisnik { KorisnickoIme = "DanicaMatic", Ime = "Danica", Prezime = "Matić", Email = "danica@gmail.com", Lozinka = PasswordHelper.Hash("danica123") };
            var aleksa = new Korisnik { KorisnickoIme = "AleksaAleksic", Ime = "Aleksa", Prezime = "Aleksić", Email = "aleksa@gmail.com", Lozinka = PasswordHelper.Hash("aleksa123") };

            db.Korisnici.AddRange(admin, masa, danica, aleksa);
            db.SaveChanges();

            // ===== RECEPTI =====
            var recepti = new List<Recept>
            {
                // --- MAŠA ---
                new Recept {
                    Naziv = "Sarma", KorisnikId = masa.Id,
                    BrojPorcija = 8, VremePripreme = "3 sata",
                    SlikaPath = "https://www.themealdb.com/images/media/meals/svprys1511176755.jpg",
                    KratakOpis = "Tradicionalno srpsko jelo — mleveno meso umotano u kiseli kupus, kuvano na laganoj vatri. Savršena zimska toplina u svakom zalogaju.",
                    Namirnice = "Meso,Namirnice",
                    Sastojci = "• 500g mlevenog mesa (junetina i svinjetina)\n• 1 glavica kiselog kupusa\n• 1 šolja pirinča\n• 1 luk\n• 2 jaja\n• So, biber, aleva paprika\n• 100g slanine",
                    NacinPripreme = "1. Propržiti sitno seckani luk.\n2. Pomešati meso, pirinač, luk, jaja i začine.\n3. Odvojiti listove kupusa i staviti fil.\n4. Zaviti i slagati u šerpu.\n5. Kuvati na laganoj vatri 2-3 sata.",
                    DijetetskiRestrikcije = "Sadrži jaja. Nije pogodno za vegetarijance.",
                    Napomene = "Ukusnija je drugi dan! Servirati sa kiselim mlekom."
                },
                new Recept {
                    Naziv = "Palačinke sa borovnicama", KorisnikId = masa.Id,
                    BrojPorcija = 4, VremePripreme = "25 minuta",
                    SlikaPath = "https://www.themealdb.com/images/media/meals/rwuyqx1511383174.jpg",
                    KratakOpis = "Mekane i sočne palačinke punjene svežim borovnicama i prelivene medom. Idealne za nedeljni doručak.",
                    Namirnice = "Mleko,Slatko,Namirnice",
                    Sastojci = "• 2 jaja\n• 300ml mleka\n• 200g brašna\n• 2 kašike šećera\n• Prstohvat soli\n• 200g borovnica\n• Med za preliv",
                    NacinPripreme = "1. Umutiti jaja sa šećerom i solju.\n2. Dodati mleko i brašno, mešati do glatke mase.\n3. Peći palačinke na zagrejanom tiganju.\n4. Puniti borovnicama i prelivati medom.",
                    DijetetskiRestrikcije = "Vegetarijansko. Sadrži gluten, jaja i laktozu.",
                    Napomene = "Mogu se koristiti i zamrznute borovnice."
                },

                // --- DANICA ---
                new Recept {
                    Naziv = "Čokoladni sufle", KorisnikId = danica.Id,
                    BrojPorcija = 2, VremePripreme = "40 minuta",
                    SlikaPath = "https://www.themealdb.com/images/media/meals/tqtywx1468317395.jpg",
                    KratakOpis = "Elegantan francuski desert sa bogatim čokoladnim ukusom. Spolja hrskav, iznutra tečan i sočan — pravi raj za ljubitelje čokolade.",
                    Namirnice = "Slatko,Namirnice",
                    Sastojci = "• 100g crne čokolade (min. 70%)\n• 2 kašike putera\n• 2 jaja\n• 120g šećera\n• 1 kašika brašna\n• Prstohvat soli\n• Kakao za kalupe",
                    NacinPripreme = "1. Otopiti čokoladu i puter na pari.\n2. Umutiti jaja i šećer do penaste mase.\n3. Sjediniti čokoladu, jaja i brašno.\n4. Kalupe premazati puterom i kakaom.\n5. Peći na 200°C tačno 10-12 minuta.",
                    DijetetskiRestrikcije = "Vegetarijansko. Za dijabetičare zameniti šećer sa zaslađivačem.",
                    Napomene = "Servirati odmah uz sladoled od vanile ili sveže voće."
                },
                new Recept {
                    Naziv = "Grčka salata", KorisnikId = danica.Id,
                    BrojPorcija = 4, VremePripreme = "10 minuta",
                    SlikaPath = "https://www.themealdb.com/images/media/meals/1550440197.jpg",
                    KratakOpis = "Osvežavajuća mediteranska salata sa povrćem, maslinama i feta sirom. Brza, lagana i savršena kao prilog ili lak obrok.",
                    Namirnice = "Namirnice",
                    Sastojci = "• 3 paradajza\n• 1 krastavac\n• 1 crveni luk\n• 200g feta sira\n• 100g crnih maslina\n• 3 kašike maslinovog ulja\n• Origano, so, biber",
                    NacinPripreme = "1. Iseckati paradajz i krastavac na krupnije komade.\n2. Crveni luk iseći na kolutiće.\n3. Pomešati povrće sa maslinima.\n4. Dodati feta sir i preliti maslinovim uljem.\n5. Posuti origanom, solju i biberom.",
                    DijetetskiRestrikcije = "Vegetarijansko. Sadrži laktozu (feta sir). Bez glutena.",
                    Napomene = "Odlična uz roštilj ili kao lagani večernji obrok."
                },

                // --- ALEKSA ---
                new Recept {
                    Naziv = "Lazanje u tiganju", KorisnikId = aleksa.Id,
                    BrojPorcija = 4, VremePripreme = "30 minuta",
                    SlikaPath = "https://www.themealdb.com/images/media/meals/sutysw1468247559.jpg",
                    KratakOpis = "Brža i jednostavnija verzija klasičnih lazanja bez pečenja u rerni. Ukusan obrok spreman za 30 minuta!",
                    Namirnice = "Meso,Namirnice",
                    Sastojci = "• 300g mlevenog mesa\n• 200g kora za lazanje\n• 400ml paradajz sosa\n• 200g rendanog sira\n• 1 luk\n• 2 čena belog luka\n• Začini po ukusu",
                    NacinPripreme = "1. Propržiti luk i beli luk na ulju.\n2. Dodati meso i pržiti dok ne porumeni.\n3. Dodati paradajz sos i začine.\n4. Polomiti kore i ubaciti u sos.\n5. Kuvati 15 min, dodati sir i topiti.",
                    DijetetskiRestrikcije = "Sadrži gluten i laktozu.",
                    Napomene = "Posuti svežim parmezanom pre serviranja."
                },
                new Recept {
                    Naziv = "Baget sa začinima", KorisnikId = aleksa.Id,
                    BrojPorcija = 6, VremePripreme = "1 sat 45 min",
                    SlikaPath = "https://www.themealdb.com/images/media/meals/sywswr1511383814.jpg",
                    KratakOpis = "Hrskav domaći hleb sa zlatno-pečenom koricom, mekanom sredinom i mediteranskim začinima. Savršen uz maslinovo ulje.",
                    Namirnice = "Namirnice",
                    Sastojci = "• 500g brašna\n• 350ml tople vode\n• 7g suvog kvasca\n• 1 kašičica soli\n• 2 kašike maslinovog ulja\n• Sušeni origano i ruzmarin",
                    NacinPripreme = "1. Pomešati brašno, kvasac i so.\n2. Dodati vodu i ulje, mesiti 10 minuta.\n3. Ostaviti testo da naraste 1 sat.\n4. Oblikovati baget i posuti začinima.\n5. Peći na 220°C oko 25 minuta.",
                    DijetetskiRestrikcije = "Veganski. Sadrži gluten.",
                    Napomene = "Savršen uz maslinovo ulje, balzamiko ili domaći sir."
                },

                // --- ADMIN ---
                new Recept {
                    Naziv = "Punjene paprike", KorisnikId = admin.Id,
                    BrojPorcija = 6, VremePripreme = "1 sat 30 min",
                    SlikaPath = "https://www.themealdb.com/images/media/meals/wvpsxx1468256321.jpg",
                    KratakOpis = "Klasično srpsko jelo — paprike punjene mlevenim mesom i pirinčem, kuvane u paradajz sosu. Neizostavni deo naše kuhinje.",
                    Namirnice = "Meso,Namirnice",
                    Sastojci = "• 6 babura paprika\n• 400g mlevenog mesa\n• 1 šolja pirinča\n• 1 luk\n• 2 jaja\n• Paradajz sos\n• So, biber, vegeta",
                    NacinPripreme = "1. Oprati paprike i izvaditi semenje.\n2. Pomešati meso, pirinač, luk, jaja i začine.\n3. Napuniti paprike filom.\n4. Slagati u šerpu i preliti paradajz sosom.\n5. Kuvati na laganoj vatri 60-90 minuta.",
                    DijetetskiRestrikcije = "Sadrži jaja. Nije za vegetarijance.",
                    Napomene = "Servirati sa kiselim mlekom i belim hlebom."
                },
                new Recept {
                    Naziv = "Čorba od pilećih bataka", KorisnikId = admin.Id,
                    BrojPorcija = 6, VremePripreme = "1 sat",
                    SlikaPath = "https://www.themealdb.com/images/media/meals/syqypv1486981727.jpg",
                    KratakOpis = "Topla i hranljiva pileća čorba sa povrćem. Savršena za hladne dane — porodični recept koji greje dušu.",
                    Namirnice = "Meso,Namirnice",
                    Sastojci = "• 4 pileća bataka\n• 2 šargarepe\n• 2 krompira\n• 1 koren peršuna\n• 1 luk\n• So, biber\n• Sveži peršun",
                    NacinPripreme = "1. Staviti piletinu u hladnu vodu i kuvati.\n2. Skinuti penu i dodati povrće.\n3. Kuvati 45-60 minuta na laganoj vatri.\n4. Izvaditi piletinu, odvojiti meso od kosti.\n5. Vratiti meso, posoliti i pobiberiti.",
                    DijetetskiRestrikcije = "Bez glutena. Nije za vegetarijance.",
                    Napomene = "Dodati rezance ili knedle za potpuni obrok."
                }
            };

            db.Recepti.AddRange(recepti);
            db.SaveChanges();

            // ===== KOMENTARI (recenzije) =====
            db.Komentari.AddRange(
                // Sarma - 5 recenzija
                new Komentar { Tekst = "Odlična sarma, baš kao od bake!", ReceptId = recepti[0].Id, KorisnikId = danica.Id },
                new Komentar { Tekst = "Kuvam svake zime, savršen recept!", ReceptId = recepti[0].Id, KorisnikId = aleksa.Id },
                new Komentar { Tekst = "5 zvezdica, preporučujem svima!", ReceptId = recepti[0].Id, KorisnikId = admin.Id },
                new Komentar { Tekst = "Malo sam dodala više paprike, fenomenalno!", ReceptId = recepti[0].Id, KorisnikId = danica.Id },
                new Komentar { Tekst = "Ukusnija je drugi dan, istina!", ReceptId = recepti[0].Id, KorisnikId = aleksa.Id },

                // Čokoladni sufle - 4 recenzije
                new Komentar { Tekst = "Predivan desert, gosti oduševljeni!", ReceptId = recepti[2].Id, KorisnikId = masa.Id },
                new Komentar { Tekst = "Malo teže za napraviti ali apsolutno vredi!", ReceptId = recepti[2].Id, KorisnikId = aleksa.Id },
                new Komentar { Tekst = "Uz sladoled od vanile je raj!", ReceptId = recepti[2].Id, KorisnikId = admin.Id },
                new Komentar { Tekst = "Savršen za romanticnu večeru!", ReceptId = recepti[2].Id, KorisnikId = masa.Id },

                // Punjene paprike - 4 recenzije
                new Komentar { Tekst = "Klasik koji nikad ne dosadi!", ReceptId = recepti[6].Id, KorisnikId = masa.Id },
                new Komentar { Tekst = "Moja omiljena srpska hrana!", ReceptId = recepti[6].Id, KorisnikId = danica.Id },
                new Komentar { Tekst = "Savršeno uz kiselo mleko!", ReceptId = recepti[6].Id, KorisnikId = aleksa.Id },
                new Komentar { Tekst = "Pravim svake nedelje!", ReceptId = recepti[6].Id, KorisnikId = masa.Id },

                // Lazanje - 3 recenzije
                new Komentar { Tekst = "Brzo i ukusno, idealno za radne dane!", ReceptId = recepti[4].Id, KorisnikId = masa.Id },
                new Komentar { Tekst = "Deca obozavaju!", ReceptId = recepti[4].Id, KorisnikId = danica.Id },
                new Komentar { Tekst = "Pravim svake nedelje.", ReceptId = recepti[4].Id, KorisnikId = admin.Id },

                // Palačinke - 3 recenzije
                new Komentar { Tekst = "Savrsene za dorucak!", ReceptId = recepti[1].Id, KorisnikId = danica.Id },
                new Komentar { Tekst = "Dodala sam i jagode, fantasticno!", ReceptId = recepti[1].Id, KorisnikId = aleksa.Id },
                new Komentar { Tekst = "Deca ih obozavaju!", ReceptId = recepti[1].Id, KorisnikId = admin.Id },

                // Čorba - 2 recenzije
                new Komentar { Tekst = "Idealna za hladne zimske dane!", ReceptId = recepti[7].Id, KorisnikId = masa.Id },
                new Komentar { Tekst = "Dodala sam rezance, savrseno!", ReceptId = recepti[7].Id, KorisnikId = danica.Id },

                // Grčka salata - 2 recenzije
                new Komentar { Tekst = "Sveza i ukusna, uz roštilj je odlicna!", ReceptId = recepti[3].Id, KorisnikId = aleksa.Id },
                new Komentar { Tekst = "Brza i lagana, cesto je pravim!", ReceptId = recepti[3].Id, KorisnikId = admin.Id },

                // Baget - 1 recenzija
                new Komentar { Tekst = "Hrskav i mirisav, kao iz pekare!", ReceptId = recepti[5].Id, KorisnikId = masa.Id }
            );
            db.SaveChanges();
        }
    }
}