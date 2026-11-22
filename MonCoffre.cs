using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace MonCoffre
{
    class StyleDef
    {
        public string Name;
        public double H, BgS, AccS, AccL;
        public StyleDef(string name, double h, double bgS, double accS, double accL)
        {
            Name = name; H = h; BgS = bgS; AccS = accS; AccL = accL;
        }
    }

    // ---------- Theme bleu nuit ----------
    static class Theme
    {
        // nom, teinte du fond, saturation du fond, saturation et luminosite de la couleur d'accent
        static readonly StyleDef[] Defs = {
            new StyleDef("Océan", 205, 0.60, 0.90, 0.50),
            new StyleDef("Ciel", 195, 0.55, 0.90, 0.62),
            new StyleDef("Cyan", 185, 0.60, 0.95, 0.50),
            new StyleDef("Indigo", 240, 0.55, 0.75, 0.65),
            new StyleDef("Menthe", 160, 0.45, 0.60, 0.62),
            new StyleDef("Émeraude", 145, 0.55, 0.70, 0.45),
            new StyleDef("Forêt", 130, 0.40, 0.50, 0.45),
            new StyleDef("Citron vert", 85, 0.45, 0.75, 0.50),
            new StyleDef("Turquoise", 172, 0.55, 0.75, 0.45),
            new StyleDef("Lavande", 255, 0.40, 0.65, 0.76),
            new StyleDef("Améthyste", 280, 0.45, 0.55, 0.62),
            new StyleDef("Fuchsia", 320, 0.55, 0.85, 0.55),
            new StyleDef("Rose bonbon", 335, 0.50, 0.85, 0.72),
            new StyleDef("Sakura", 345, 0.40, 0.70, 0.80),
            new StyleDef("Jaune", 50, 0.55, 0.95, 0.55),
            new StyleDef("Or", 42, 0.55, 0.85, 0.50),
            new StyleDef("Corail", 12, 0.55, 0.85, 0.65),
            new StyleDef("Bordeaux", 345, 0.55, 0.70, 0.45),
            new StyleDef("Rouge sang", 355, 0.60, 0.85, 0.46),
            new StyleDef("Gris acier", 210, 0.14, 0.18, 0.62),
            new StyleDef("Graphite", 220, 0.07, 0.10, 0.55),
            new StyleDef("Chocolat", 25, 0.40, 0.50, 0.50),
            new StyleDef("Cyan néon", 180, 0.60, 1.00, 0.50),
            new StyleDef("Vert néon", 120, 0.55, 1.00, 0.50),
            new StyleDef("Rose néon", 315, 0.55, 1.00, 0.62)
        };

        public static readonly string[] Names = BuildNames();
        public static string Current = "Bleu";
        public static bool IsLight;
        public static Color CustomColor = Color.FromArgb(59, 130, 246);

        static string[] BuildNames()
        {
            List<string> l = new List<string> { "Bleu", "Vert", "Rouge", "Gris acier", "Turquoise", "Graphite", "Violet", "Orange", "Noir" };
            foreach (StyleDef d in Defs) if (!l.Contains(d.Name)) l.Add(d.Name);
            l.Add("Blanc");
            return l.ToArray();
        }

        // Styles gratuits : tout le reste (et la couleur libre) demande la licence Pro.
        public static bool IsFree(string name)
        {
            return name == "Bleu" || name == "Vert" || name == "Rouge" || name == "Gris acier" || name == "Turquoise" || name == "Graphite";
        }

        static StyleDef FindDef(string name)
        {
            foreach (StyleDef d in Defs) if (d.Name == name) return d;
            return null;
        }

        static void SetFromHue(string name, double h, double bgS, Color accent)
        {
            Current = name;
            BgC = Hsl(h, bgS, 0.08);
            PanelC = Hsl(h, bgS * 0.82, 0.13);
            FieldC = Hsl(h, bgS, 0.055);
            AltC = Hsl(h, bgS * 0.9, 0.10);
            BorderC = Hsl(h, bgS * 0.65, 0.26);
            SelectC = Hsl(h, bgS * 0.9, 0.28);
            TextC = Color.FromArgb(232, 236, 242);
            MutedC = Hsl(h, Math.Min(0.25, bgS), 0.62);
            AccentC = accent;
        }

        public static Color BgC = Color.FromArgb(10, 18, 34);
        public static Color PanelC = Color.FromArgb(16, 28, 50);
        public static Color FieldC = Color.FromArgb(8, 14, 27);
        public static Color AltC = Color.FromArgb(12, 22, 40);
        public static Color BorderC = Color.FromArgb(38, 58, 94);
        public static Color TextC = Color.FromArgb(226, 234, 248);
        public static Color MutedC = Color.FromArgb(135, 152, 182);
        public static Color AccentC = Color.FromArgb(59, 130, 246);
        public static Color SelectC = Color.FromArgb(30, 64, 120);
        public static Color OkC = Color.FromArgb(52, 211, 153);

        static Color Hsl(double h, double s, double l)
        {
            double c = (1 - Math.Abs(2 * l - 1)) * s;
            double hp = h / 60.0;
            double x = c * (1 - Math.Abs(hp % 2 - 1));
            double r = 0, g = 0, b = 0;
            if (hp < 1) { r = c; g = x; }
            else if (hp < 2) { r = x; g = c; }
            else if (hp < 3) { g = c; b = x; }
            else if (hp < 4) { g = x; b = c; }
            else if (hp < 5) { r = x; b = c; }
            else { r = c; b = x; }
            double m = l - c / 2;
            return Color.FromArgb((int)Math.Round((r + m) * 255), (int)Math.Round((g + m) * 255), (int)Math.Round((b + m) * 255));
        }

        // couleur d'accent d'un style (aussi utilisee pour l'apercu dans la fenetre de choix)
        public static Color PreviewColor(string name)
        {
            StyleDef d = FindDef(name);
            if (d != null) return Hsl(d.H, d.AccS, d.AccL);
            if (name == "Perso") return CustomColor;
            if (name == "Blanc") return Color.FromArgb(243, 245, 249);
            switch (name)
            {
                case "Vert": return Hsl(152, 0.65, 0.45);
                case "Violet": return Hsl(268, 0.80, 0.65);
                case "Orange": return Hsl(28, 0.95, 0.55);
                case "Rouge": return Hsl(0, 0.80, 0.58);
                case "Noir": return Color.FromArgb(28, 28, 28);
                default: return Color.FromArgb(59, 130, 246);
            }
        }

        public static void SetStyle(string name)
        {
            IsLight = false;
            OkC = Color.FromArgb(52, 211, 153);
            StyleDef def = FindDef(name);
            if (def != null)
            {
                SetFromHue(name, def.H, def.BgS, Hsl(def.H, def.AccS, def.AccL));
                return;
            }
            if (name == "Perso")
            {
                Color pc = CustomColor;
                double ph = pc.GetHue();
                double pl = pc.GetBrightness();
                // une couleur trop sombre ou trop claire serait illisible : on l'ajuste
                Color acc = (pl < 0.35 || pl > 0.85) ? Hsl(ph, Math.Max(0.5, pc.GetSaturation()), 0.55) : pc;
                SetFromHue("Perso", ph, 0.55, acc);
                return;
            }
            if (name == "Blanc")
            {
                Current = "Blanc";
                IsLight = true;
                BgC = Color.FromArgb(243, 245, 249);
                PanelC = Color.FromArgb(255, 255, 255);
                FieldC = Color.FromArgb(255, 255, 255);
                AltC = Color.FromArgb(237, 241, 248);
                BorderC = Color.FromArgb(205, 212, 225);
                TextC = Color.FromArgb(25, 30, 45);
                MutedC = Color.FromArgb(100, 110, 130);
                AccentC = Color.FromArgb(59, 130, 246);
                SelectC = Color.FromArgb(205, 223, 252);
                OkC = Color.FromArgb(22, 163, 74);
                return;
            }
            Current = name;
            double h = 220;
            switch (name)
            {
                case "Vert": h = 152; break;
                case "Violet": h = 268; break;
                case "Orange": h = 28; break;
                case "Rouge": h = 0; break;
                case "Noir":
                    BgC = Color.FromArgb(10, 10, 10);
                    PanelC = Color.FromArgb(22, 22, 22);
                    FieldC = Color.FromArgb(5, 5, 5);
                    AltC = Color.FromArgb(15, 15, 15);
                    BorderC = Color.FromArgb(55, 55, 55);
                    TextC = Color.FromArgb(235, 235, 235);
                    MutedC = Color.FromArgb(150, 150, 150);
                    AccentC = Color.FromArgb(150, 150, 150);
                    SelectC = Color.FromArgb(50, 50, 50);
                    return;
                default:
                    Current = "Bleu";
                    BgC = Color.FromArgb(10, 18, 34);
                    PanelC = Color.FromArgb(16, 28, 50);
                    FieldC = Color.FromArgb(8, 14, 27);
                    AltC = Color.FromArgb(12, 22, 40);
                    BorderC = Color.FromArgb(38, 58, 94);
                    TextC = Color.FromArgb(226, 234, 248);
                    MutedC = Color.FromArgb(135, 152, 182);
                    AccentC = Color.FromArgb(59, 130, 246);
                    SelectC = Color.FromArgb(30, 64, 120);
                    return;
            }
            BgC = Hsl(h, 0.55, 0.08);
            PanelC = Hsl(h, 0.45, 0.13);
            FieldC = Hsl(h, 0.55, 0.055);
            AltC = Hsl(h, 0.50, 0.10);
            BorderC = Hsl(h, 0.35, 0.26);
            SelectC = Hsl(h, 0.50, 0.28);
            TextC = Color.FromArgb(232, 236, 242);
            MutedC = Hsl(h, 0.20, 0.62);
            AccentC = PreviewColor(name);
        }

        static string StyleFile
        {
            get { return System.IO.Path.Combine(Vault.DataDir, "style.txt"); }
        }

        public static void Load()
        {
            try
            {
                if (File.Exists(StyleFile))
                {
                    string[] ln = File.ReadAllLines(StyleFile);
                    if (ln.Length > 1)
                    {
                        try
                        {
                            int v = Convert.ToInt32(ln[1].Trim(), 16);
                            CustomColor = Color.FromArgb((v >> 16) & 255, (v >> 8) & 255, v & 255);
                        }
                        catch (Exception) { }
                    }
                    if (ln.Length > 0)
                    {
                        string n = ln[0].Trim();
                        if (n == "Perso" || Array.IndexOf(Names, n) >= 0) SetStyle(n);
                    }
                }
            }
            catch (Exception) { }
        }

        public static void Save()
        {
            try
            {
                Directory.CreateDirectory(Vault.DataDir);
                string hex = ((CustomColor.R << 16) | (CustomColor.G << 8) | CustomColor.B).ToString("X6");
                File.WriteAllText(StyleFile, Current + "\n" + hex);
            }
            catch (Exception) { }
        }

        [DllImport("dwmapi.dll")]
        static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int val, int size);

        public static void DarkTitle(Form f)
        {
            // logo de l'application (celui de l'exe) sur toutes les fenetres
            try { f.Icon = System.Drawing.Icon.ExtractAssociatedIcon(Application.ExecutablePath); } catch (Exception) { }
            f.HandleCreated += (s, e) => UpdateTitleBar(f);
        }

        // barre de titre sombre (ou claire pour le style Blanc)
        public static void UpdateTitleBar(Form f)
        {
            try
            {
                if (!f.IsHandleCreated) return;
                int v = IsLight ? 0 : 1;
                if (DwmSetWindowAttribute(f.Handle, 20, ref v, 4) != 0) DwmSetWindowAttribute(f.Handle, 19, ref v, 4);
            }
            catch (Exception) { }
        }

        public static void Apply(Control c)
        {
            c.ForeColor = TextC;
            if (c is Form || c is Panel) c.BackColor = BgC;
            else if (c is Label || c is CheckBox) c.BackColor = Color.Transparent;
            else if (c is TextBox)
            {
                TextBox t = (TextBox)c;
                t.BackColor = FieldC;
                t.BorderStyle = BorderStyle.FixedSingle;
            }
            else if (c is ListBox)
            {
                ListBox l = (ListBox)c;
                l.BackColor = PanelC;
                l.BorderStyle = BorderStyle.None;
            }
            else if (c is Button)
            {
                Button b = (Button)c;
                b.UseVisualStyleBackColor = false;
                b.FlatStyle = FlatStyle.Flat;
                b.BackColor = PanelC;
                b.FlatAppearance.BorderColor = BorderC;
                b.FlatAppearance.MouseOverBackColor = SelectC;
                b.FlatAppearance.MouseDownBackColor = AccentC;
                b.Cursor = Cursors.Hand;
            }
            else if (c is DataGridView)
            {
                DataGridView g = (DataGridView)c;
                g.EnableHeadersVisualStyles = false;
                g.BackgroundColor = BgC;
                g.BorderStyle = BorderStyle.None;
                g.GridColor = BorderC;
                g.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                g.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                g.ColumnHeadersDefaultCellStyle.BackColor = PanelC;
                g.ColumnHeadersDefaultCellStyle.ForeColor = MutedC;
                g.ColumnHeadersDefaultCellStyle.SelectionBackColor = PanelC;
                g.ColumnHeadersDefaultCellStyle.SelectionForeColor = MutedC;
                g.ColumnHeadersDefaultCellStyle.Padding = new Padding(4, 6, 4, 6);
                g.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                g.DefaultCellStyle.BackColor = FieldC;
                g.DefaultCellStyle.ForeColor = TextC;
                g.DefaultCellStyle.SelectionBackColor = SelectC;
                g.DefaultCellStyle.SelectionForeColor = TextC;
                g.AlternatingRowsDefaultCellStyle.BackColor = AltC;
                g.AlternatingRowsDefaultCellStyle.ForeColor = TextC;
                g.AlternatingRowsDefaultCellStyle.SelectionBackColor = SelectC;
                g.AlternatingRowsDefaultCellStyle.SelectionForeColor = TextC;
            }
            foreach (Control child in c.Controls) Apply(child);
        }
    }

    class Row
    {
        public string Label = "";
        public string Value = "";
        public bool Secret;
    }

    class Box
    {
        public string Title = "Nouvelle case";
        public string Icon = "";
        public List<Row> Rows = new List<Row>();
        public override string ToString() { return Title; }
    }

    // element supprime (case entiere ou ligne), garde 30 jours dans la corbeille
    class TrashItem
    {
        public bool IsRow;
        public string ParentTitle = "";
        public Box B;
        public Row R;
        public long Ticks;
    }

    // ---------- Licence professionnelle : activation par cle, verification hors ligne ----------
    static class License
    {
        public const string Product = "Mon Coffre Pro";
        public const string Version = "1.0";
        const string Alphabet = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
        const string RegPath = @"Software\MonCoffre";

        // Version publique : le vrai secret de licence n'est volontairement pas dans ce fichier.
        // Une version compilee d'ici n'accepte donc pas les cles de l'editeur officiel.
        static readonly byte[] Secret = Encoding.UTF8.GetBytes("SECRET-PUBLIC-SANS-VALEUR");

        public static string Holder = "";
        public static string Key = "";
        public static bool Activated;

        public const string Terms =
            "CONTRAT DE LICENCE UTILISATEUR — MON COFFRE PRO\r\n\r\n" +
            "1. Objet\r\nLe présent contrat encadre l'utilisation du logiciel « Mon Coffre Pro » (le « Logiciel »), un gestionnaire de mots de passe qui conserve des informations de façon chiffrée sur l'ordinateur de l'utilisateur.\r\n\r\n" +
            "2. Licence professionnelle\r\nSous réserve du respect des présentes conditions, l'Éditeur concède au titulaire de la clé de licence un droit d'utilisation personnel, non exclusif et non transférable du Logiciel, y compris dans un cadre professionnel, sur les postes autorisés par la licence acquise. Chaque clé de licence ne peut être activée qu'une seule fois : en cas de désactivation, de réinstallation ou de perte, une nouvelle clé doit être demandée à l'Éditeur.\r\n\r\n" +
            "3. Restrictions\r\nIl est interdit de copier le Logiciel pour le redistribuer, de revendre ou de partager une clé de licence, de contourner ou de désactiver le mécanisme d'activation, ou de décompiler le Logiciel, sauf dans la mesure permise par la loi.\r\n\r\n" +
            "4. Données et confidentialité\r\nLes données saisies sont enregistrées uniquement sur l'ordinateur de l'utilisateur, chiffrées avec son mot de passe maître. Le Logiciel ne transmet aucune donnée à l'Éditeur ni à un tiers. L'Éditeur n'a donc aucun moyen d'accéder aux données ni de retrouver un mot de passe maître oublié : l'utilisateur est responsable de conserver son mot de passe maître et ses sauvegardes.\r\n\r\n" +
            "5. Garantie et responsabilité\r\nLe Logiciel est fourni « en l'état », sans garantie d'aucune sorte. L'Éditeur ne peut être tenu responsable d'une perte de données, notamment en cas d'oubli du mot de passe maître, de panne ou de suppression de fichiers. L'utilisateur est invité à effectuer des sauvegardes régulières.\r\n\r\n" +
            "6. Durée et résiliation\r\nLa licence est accordée pour une durée illimitée, sauf mention contraire lors de l'achat. Elle prend fin automatiquement en cas de non-respect des présentes conditions.\r\n\r\n" +
            "7. Droit applicable\r\nLe présent contrat est soumis au droit français.";

        static string FilePath
        {
            get { return System.IO.Path.Combine(Vault.DataDir, "licence.dat"); }
        }

        static string Norm(string name) { return (name ?? "").Trim().ToLowerInvariant(); }

        static string Check(string name, string a, string b, string c)
        {
            using (HMACSHA256 h = new HMACSHA256(Secret))
            {
                byte[] m = h.ComputeHash(Encoding.UTF8.GetBytes(Norm(name) + "|" + a + b + c));
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < 5; i++) sb.Append(Alphabet[m[i] % 32]);
                return sb.ToString();
            }
        }

        // Une cle a la forme MCPR-AAAAA-BBBBB-CCCCC-DDDDD ; le dernier groupe depend du nom du titulaire.
        public static bool Validate(string name, string key)
        {
            if (Norm(name).Length < 2 || string.IsNullOrEmpty(key)) return false;
            string[] p = key.Trim().ToUpperInvariant().Split('-');
            if (p.Length != 5 || p[0] != "MCPR") return false;
            for (int i = 1; i < 5; i++)
            {
                if (p[i].Length != 5) return false;
                foreach (char ch in p[i]) if (Alphabet.IndexOf(ch) < 0) return false;
            }
            return Check(name, p[1], p[2], p[3]) == p[4];
        }

        public static void Load()
        {
            Activated = false; Holder = ""; Key = "";
            string raw = null;
            try { if (File.Exists(FilePath)) raw = File.ReadAllText(FilePath); } catch (Exception) { }
            if (string.IsNullOrEmpty(raw))
            {
                try
                {
                    using (Microsoft.Win32.RegistryKey k = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(RegPath))
                        if (k != null) raw = k.GetValue("lic") as string;
                }
                catch (Exception) { }
            }
            if (string.IsNullOrEmpty(raw)) return;
            string[] ln = raw.Replace("\r", "").Split('\n');
            if (ln.Length < 2) return;
            string h0 = ln[0].Trim(), k0 = ln[1].Trim().ToUpperInvariant();
            // La licence deja enregistree reste valable apres une mise a jour du programme :
            // soit son sceau (3e ligne) est bon, soit (anciens fichiers sans sceau) la cle est valide.
            bool sealOk = ln.Length >= 3 && ln[2].Trim() == Seal(h0, k0);
            if (sealOk || Validate(h0, k0))
            {
                Holder = h0;
                Key = k0;
                Activated = true;
                if (!sealOk) Store();   // ajoute le sceau aux anciennes licences
            }
        }

        // Sceau independant du Secret de generation : un changement de Secret ne desactive pas une licence deja activee.
        static string Seal(string holder, string key)
        {
            using (SHA256 sh = SHA256.Create())
            {
                byte[] h = sh.ComputeHash(Encoding.UTF8.GetBytes("seal-v1|" + Norm(holder) + "|" + key.Trim().ToUpperInvariant()));
                return BitConverter.ToString(h).Replace("-", "").Substring(0, 32);
            }
        }

        static void Store()
        {
            string val = Holder + "\n" + Key + "\n" + Seal(Holder, Key);
            try { Directory.CreateDirectory(Vault.DataDir); File.WriteAllText(FilePath, val); } catch (Exception) { }
            try
            {
                using (Microsoft.Win32.RegistryKey k = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(RegPath))
                    k.SetValue("lic", val);
            }
            catch (Exception) { }
        }

        // ---- cles deja utilisees : une cle ne peut etre activee qu'une seule fois ----
        // La liste (empreintes, pas les cles) est gardee en 3 endroits : un fichier dans AppData,
        // un fichier dans LocalAppData et le registre. Une cle desactivee reste donc "utilisee".
        static string UsedFile1
        {
            get { return System.IO.Path.Combine(Vault.DataDir, "licence.used"); }
        }

        static string UsedFile2
        {
            get { return System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MonCoffre", "sys.dat"); }
        }

        static string KeyHash(string key)
        {
            using (SHA256 sh = SHA256.Create())
            {
                byte[] h = sh.ComputeHash(Encoding.UTF8.GetBytes("used|" + key.Trim().ToUpperInvariant()));
                return BitConverter.ToString(h).Replace("-", "").Substring(0, 24);
            }
        }

        static HashSet<string> LoadUsed()
        {
            HashSet<string> set = new HashSet<string>();
            string[] files = { UsedFile1, UsedFile2 };
            foreach (string fp in files)
            {
                try
                {
                    if (File.Exists(fp))
                        foreach (string ln in File.ReadAllLines(fp))
                        {
                            string t1 = ln.Trim();
                            if (t1.Length > 0) set.Add(t1);
                        }
                }
                catch (Exception) { }
            }
            try
            {
                using (Microsoft.Win32.RegistryKey k = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(RegPath))
                {
                    string r = k == null ? null : k.GetValue("used") as string;
                    if (!string.IsNullOrEmpty(r))
                        foreach (string t2 in r.Split(';'))
                            if (t2.Length > 0) set.Add(t2);
                }
            }
            catch (Exception) { }
            return set;
        }

        static void SaveUsed(HashSet<string> set)
        {
            string text = string.Join("\n", new List<string>(set).ToArray());
            string[] files = { UsedFile1, UsedFile2 };
            foreach (string fp in files)
            {
                try
                {
                    Directory.CreateDirectory(System.IO.Path.GetDirectoryName(fp));
                    File.WriteAllText(fp, text);
                }
                catch (Exception) { }
            }
            try
            {
                using (Microsoft.Win32.RegistryKey k = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(RegPath))
                    k.SetValue("used", string.Join(";", new List<string>(set).ToArray()));
            }
            catch (Exception) { }
        }

        // ---- activation en ligne : le serveur retient les cles deja utilisees (toutes machines confondues) ----
        // Colle ici l'adresse de ton serveur (voir LISEZ-MOI-SERVEUR.txt), par ex. "https://mon-coffre.xxx.workers.dev".
        // Laisse vide pour rester en mode hors ligne (la protection ne vaut alors que pour cet ordinateur).
        const string ServerUrl = "";

        // 0 = ok (cle consommee), 2 = cle deja utilisee, 3 = serveur injoignable
        static int ServerClaim(string name, string key)
        {
            if (ServerUrl.Length == 0) return 0;
            try
            {
                try { ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072; } catch (Exception) { }   // TLS 1.2
                HttpWebRequest rq = (HttpWebRequest)WebRequest.Create(ServerUrl.TrimEnd('/') + "/activate");
                rq.Method = "POST";
                rq.ContentType = "application/json";
                rq.Timeout = 15000;
                string body = "{\"key\":\"" + key + "\",\"name\":\"" + Norm(name).Replace("\\", "").Replace("\"", "") + "\",\"pc\":\"" + Environment.MachineName.Replace("\\", "").Replace("\"", "") + "\"}";
                byte[] data = Encoding.UTF8.GetBytes(body);
                rq.ContentLength = data.Length;
                using (Stream s = rq.GetRequestStream()) s.Write(data, 0, data.Length);
                using (HttpWebResponse rs = (HttpWebResponse)rq.GetResponse())
                using (StreamReader sr = new StreamReader(rs.GetResponseStream()))
                {
                    string t = sr.ReadToEnd();
                    if (t.Contains("\"ok\"")) return 0;
                    if (t.Contains("\"used\"")) return 2;
                    return 3;
                }
            }
            catch (WebException ex)
            {
                try
                {
                    HttpWebResponse er = ex.Response as HttpWebResponse;
                    if (er != null)
                        using (StreamReader sr2 = new StreamReader(er.GetResponseStream()))
                            if (sr2.ReadToEnd().Contains("\"used\"")) return 2;
                }
                catch (Exception) { }
                return 3;
            }
            catch (Exception) { return 3; }
        }

        // 0 = activee, 1 = cle ou nom invalide, 2 = cle deja utilisee, 3 = serveur injoignable
        public static int Activate(string name, string key)
        {
            if (!Validate(name, key)) return 1;
            string nk = key.Trim().ToUpperInvariant();
            bool same = Activated && Key == nk;   // meme cle deja active sur cet ordinateur : rien a consommer
            if (!same)
            {
                HashSet<string> used = LoadUsed();
                string kh = KeyHash(nk);
                if (used.Contains(kh)) return 2;
                int srv = ServerClaim(name, nk);   // la cle est consommee cote serveur AVANT de l'accepter ici
                if (srv != 0) return srv;
                used.Add(kh);
                SaveUsed(used);
            }
            Holder = name.Trim();
            Key = nk;
            Activated = true;
            Store();
            return 0;
        }

        public static void Deactivate()
        {
            Holder = ""; Key = ""; Activated = false;
            try { if (File.Exists(FilePath)) File.Delete(FilePath); } catch (Exception) { }
            try
            {
                using (Microsoft.Win32.RegistryKey k = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(RegPath, true))
                    if (k != null) k.DeleteValue("lic", false);
            }
            catch (Exception) { }
        }

        public static string MaskedKey()
        {
            string[] p = Key.Split('-');
            if (p.Length != 5) return Key;
            return p[0] + "-" + p[1] + "-•••••-•••••-" + p[4];
        }
    }

    // ---------- Reglages de securite (enregistres dans prefs.txt) ----------
    static class Prefs
    {
        public static bool AntiCapture;
        public static bool BlurOnLeave;

        [DllImport("user32.dll")]
        static extern bool SetWindowDisplayAffinity(IntPtr hWnd, uint dwAffinity);

        static string PrefFile
        {
            get { return System.IO.Path.Combine(Vault.DataDir, "prefs.txt"); }
        }

        public static void Load()
        {
            try
            {
                if (!File.Exists(PrefFile)) return;
                foreach (string line in File.ReadAllLines(PrefFile))
                {
                    string[] kv = line.Split('=');
                    if (kv.Length != 2) continue;
                    bool on = kv[1].Trim() == "1";
                    if (kv[0].Trim() == "anticapture") AntiCapture = on;
                    else if (kv[0].Trim() == "blur") BlurOnLeave = on;
                }
            }
            catch (Exception) { }
        }

        public static void Save()
        {
            try
            {
                Directory.CreateDirectory(Vault.DataDir);
                File.WriteAllText(PrefFile, "anticapture=" + (AntiCapture ? "1" : "0") + "\nblur=" + (BlurOnLeave ? "1" : "0") + "\n");
            }
            catch (Exception) { }
        }

        // cache la fenetre des captures d'ecran / du partage d'ecran
        public static void ApplyCapture(Form f)
        {
            try
            {
                if (!f.IsHandleCreated) return;
                if (AntiCapture)
                {
                    if (!SetWindowDisplayAffinity(f.Handle, 0x11)) SetWindowDisplayAffinity(f.Handle, 1);
                }
                else SetWindowDisplayAffinity(f.Handle, 0);
            }
            catch (Exception) { }
        }
    }

    // ---------- Force d'un mot de passe ----------
    static class Strength
    {
        public static int Evaluate(string v, out string label, out Color col)
        {
            if (string.IsNullOrEmpty(v)) { label = "—"; col = Theme.MutedC; return 0; }
            bool lo = false, up = false, di = false, sy = false;
            HashSet<char> uniq = new HashSet<char>();
            foreach (char c in v)
            {
                uniq.Add(c);
                if (c >= 'a' && c <= 'z') lo = true;
                else if (c >= 'A' && c <= 'Z') up = true;
                else if (c >= '0' && c <= '9') di = true;
                else sy = true;
            }
            int pool = (lo ? 26 : 0) + (up ? 26 : 0) + (di ? 10 : 0) + (sy ? 33 : 0);
            double eff = Math.Min(v.Length, uniq.Count * 2.0);
            double bits = eff * Math.Log(Math.Max(pool, 2), 2);
            string low = v.ToLowerInvariant();
            string[] common = { "password", "motdepasse", "azerty", "qwerty", "123456", "admin", "bonjour", "soleil", "abc123", "111111", "000000" };
            foreach (string w in common)
                if (low.Contains(w)) { bits = Math.Min(bits, 24); break; }
            int pct = (int)Math.Min(100.0, Math.Round(bits / 80.0 * 100.0));
            if (bits < 28) { label = "Très faible"; col = Color.FromArgb(239, 68, 68); }
            else if (bits < 40) { label = "Faible"; col = Color.FromArgb(249, 115, 22); }
            else if (bits < 56) { label = "Moyen"; col = Color.FromArgb(234, 179, 8); }
            else if (bits < 72) { label = "Fort"; col = Color.FromArgb(132, 204, 22); }
            else { label = "Très fort"; col = Color.FromArgb(34, 197, 94); }
            return Math.Max(pct, 6);
        }
    }

    class StrengthBar : Control
    {
        int pct;
        Color fill = Color.Gray;

        public StrengthBar()
        {
            DoubleBuffered = true;
            Size = new Size(180, 12);
        }

        public void Set(int p, Color c)
        {
            pct = p; fill = c;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(Theme.FieldC);
            int w = (int)(Width * pct / 100.0);
            if (w > 0)
                using (SolidBrush b = new SolidBrush(fill)) e.Graphics.FillRectangle(b, 0, 0, w, Height);
            using (Pen p = new Pen(Theme.BorderC)) e.Graphics.DrawRectangle(p, 0, 0, Width - 1, Height - 1);
        }
    }

    // ---------- Fond d'ecran : image perso ou animation ----------
    static class Backdrop
    {
        public static readonly string[] Modes = { "aucun", "image", "bulles", "aurore", "matrix",
            "neige", "etoiles", "pluie", "vagues", "braises", "lucioles", "bokeh", "grille", "degrade", "ondes" };
        public static string Mode = "aucun";

        static Bitmap frame, small, img;
        static int W, H, tick;
        static Random rnd = new Random();
        static string lastMode = "";

        // ondes
        static float[] rx = new float[5], ry = new float[5];
        static int[] rs = new int[5];
        static bool rInit;

        // bulles et autres particules (neige, etoiles, pluie, braises, lucioles, bokeh)
        const int NB = 130;
        const int BUB = 55;
        static float[] bx = new float[NB], by = new float[NB], br = new float[NB], bv = new float[NB], bp = new float[NB];
        static int[] ba = new int[NB];
        static bool bInit;

        // matrix
        const int CW = 20;
        const string Glyphs = "01ABCDEFGHJKLMNPRSTUVXYZ#$%&*+=<>?";
        static int mCols;
        static float[] my, mv;
        static int[] ml;
        static char[][] mc;
        static Font mFont;

        static string ModeFile { get { return System.IO.Path.Combine(Vault.DataDir, "fond.txt"); } }
        static string ImgFile { get { return System.IO.Path.Combine(Vault.DataDir, "fond.img"); } }

        public static bool Animated { get { return Mode != "aucun" && Mode != "image"; } }

        // Fonds gratuits : tous les autres demandent la licence Pro.
        public static bool IsFree(string m) { return m == "aucun" || m == "image" || m == "bulles" || m == "matrix"; }

        static Color Mix(Color a, Color b, double t)
        {
            return Color.FromArgb((int)(a.R + (b.R - a.R) * t), (int)(a.G + (b.G - a.G) * t), (int)(a.B + (b.B - a.B) * t));
        }

        public static void Load()
        {
            try
            {
                if (File.Exists(ModeFile))
                {
                    string m = File.ReadAllText(ModeFile).Trim();
                    if (Array.IndexOf(Modes, m) >= 0) Mode = m;
                }
                if (Mode == "image" && !(File.Exists(ImgFile) && LoadImage(ImgFile))) Mode = "aucun";
            }
            catch (Exception) { Mode = "aucun"; }
        }

        public static void Save()
        {
            try
            {
                Directory.CreateDirectory(Vault.DataDir);
                File.WriteAllText(ModeFile, Mode);
            }
            catch (Exception) { }
        }

        static bool LoadImage(string file)
        {
            try
            {
                byte[] data = File.ReadAllBytes(file);
                using (MemoryStream ms = new MemoryStream(data))
                using (Image tmp = Image.FromStream(ms))
                {
                    double sc = Math.Min(1.0, 1920.0 / Math.Max(tmp.Width, tmp.Height));
                    int w = Math.Max(1, (int)(tmp.Width * sc));
                    int h = Math.Max(1, (int)(tmp.Height * sc));
                    Bitmap nb = new Bitmap(w, h);
                    using (Graphics g = Graphics.FromImage(nb))
                    {
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.DrawImage(tmp, 0, 0, w, h);
                    }
                    if (img != null) img.Dispose();
                    img = nb;
                }
                return true;
            }
            catch (Exception) { return false; }
        }

        // lit l'image choisie ; si elle est valide, on en garde une copie dans le dossier de l'appli
        public static bool SetImage(string path)
        {
            if (!LoadImage(path)) return false;
            try
            {
                Directory.CreateDirectory(Vault.DataDir);
                File.Copy(path, ImgFile, true);
            }
            catch (Exception) { return false; }
            return true;
        }

        public static void Resize(int w, int h)
        {
            if (w < 2 || h < 2) return;
            if (w == W && h == H && frame != null) return;
            W = w; H = h;
            if (frame != null) frame.Dispose();
            frame = new Bitmap(W, H);
            if (small != null) small.Dispose();
            small = new Bitmap(Math.Max(2, W / 2), Math.Max(2, H / 2));
            bInit = false;
            mCols = 0;
        }

        // dessine une image du fond (appelee par le minuteur pour les animations)
        public static void Render()
        {
            if (frame == null || Mode == "aucun") return;
            tick++;
            if (lastMode != Mode)
            {
                lastMode = Mode;
                bInit = false; rInit = false; mCols = 0; my = null;
            }
            using (Graphics g = Graphics.FromImage(frame))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                switch (Mode)
                {
                    case "image": RenderImage(g); break;
                    case "bulles": RenderBubbles(g); break;
                    case "aurore": RenderAurora(g); break;
                    case "matrix": RenderMatrix(g); break;
                    case "vagues": RenderWaves(g); break;
                    case "grille": RenderGrid(g); break;
                    case "degrade": RenderGradient(g); break;
                    case "ondes": RenderRipples(g); break;
                    default: RenderParticles(g, Mode); break;
                }
            }
        }

        // ----- nouveaux fonds -----
        static int ParticleCount(string kind)
        {
            switch (kind)
            {
                case "neige": return 110;
                case "etoiles": return 120;
                case "pluie": return 100;
                case "braises": return 90;
                case "lucioles": return 40;
                default: return 22;
            }
        }

        static void InitParticles(string kind)
        {
            int n = ParticleCount(kind);
            for (int i = 0; i < n; i++)
            {
                bx[i] = rnd.Next(W);
                by[i] = rnd.Next(H);
                bp[i] = (float)(rnd.NextDouble() * 6.28);
                float q = (float)rnd.NextDouble();
                switch (kind)
                {
                    case "neige": br[i] = 1.5f + q * 2.5f; bv[i] = 0.5f + q * 1.1f; ba[i] = 120 + rnd.Next(100); break;
                    case "etoiles": br[i] = 0.8f + q * 1.7f; bv[i] = 0.02f + q * 0.05f; ba[i] = 255; break;
                    case "pluie": br[i] = 10 + q * 12f; bv[i] = 9 + q * 8f; ba[i] = 50 + rnd.Next(80); break;
                    case "braises": br[i] = 1 + q * 2.5f; bv[i] = 0.6f + q * 1.6f; ba[i] = 100 + rnd.Next(150); break;
                    case "lucioles": br[i] = 2 + q * 2f; bv[i] = 0.3f + q * 0.6f; ba[i] = 255; break;
                    default: br[i] = 18 + q * 55f; bv[i] = 0.1f + q * 0.35f; ba[i] = 18 + rnd.Next(30); break;
                }
            }
            bInit = true;
        }

        static void RenderParticles(Graphics g, string kind)
        {
            Color acc = Theme.AccentC;
            Color top = Theme.BgC, bottom = Theme.BgC;
            switch (kind)
            {
                case "neige": bottom = Mix(Theme.BgC, Color.White, 0.10); break;
                case "pluie": top = Mix(Theme.BgC, Color.Black, 0.3); bottom = Mix(Theme.BgC, acc, 0.12); break;
                case "etoiles": top = Mix(Theme.BgC, Color.Black, 0.5); break;
                case "braises": top = Mix(Theme.BgC, Color.Black, 0.4); bottom = Mix(Theme.BgC, acc, 0.35); break;
                case "lucioles": top = Mix(Theme.BgC, Color.Black, 0.45); bottom = Mix(Theme.BgC, Color.Black, 0.15); break;
                default: bottom = Mix(Theme.BgC, acc, 0.15); break;
            }
            using (LinearGradientBrush lg = new LinearGradientBrush(new Rectangle(0, 0, W, H), top, bottom, 90f))
                g.FillRectangle(lg, 0, 0, W, H);
            if (!bInit) InitParticles(kind);
            int n = ParticleCount(kind);
            Color light = Mix(Color.White, acc, 0.15);
            for (int i = 0; i < n; i++)
            {
                switch (kind)
                {
                    case "neige":
                        by[i] += bv[i];
                        bx[i] += (float)Math.Sin(tick * 0.02 + bp[i]) * 0.5f;
                        if (by[i] > H + 5) { by[i] = -5; bx[i] = rnd.Next(W); }
                        using (SolidBrush sb = new SolidBrush(Color.FromArgb(ba[i], light)))
                            g.FillEllipse(sb, bx[i] - br[i], by[i] - br[i], br[i] * 2, br[i] * 2);
                        break;
                    case "etoiles":
                        bx[i] += bv[i];
                        if (bx[i] > W + 5) bx[i] = -5;
                        int a = 70 + (int)(150 * Math.Abs(Math.Sin(tick * 0.04 + bp[i])));
                        using (SolidBrush sb = new SolidBrush(Color.FromArgb(Math.Min(255, a), light)))
                            g.FillEllipse(sb, bx[i] - br[i], by[i] - br[i], br[i] * 2, br[i] * 2);
                        break;
                    case "pluie":
                        by[i] += bv[i];
                        bx[i] -= bv[i] * 0.25f;
                        if (by[i] > H + 20) { by[i] = -20; bx[i] = rnd.Next(W + 200); }
                        using (Pen p = new Pen(Color.FromArgb(ba[i], Mix(Color.White, acc, 0.4)), 1f))
                            g.DrawLine(p, bx[i] + br[i] * 0.25f, by[i] - br[i], bx[i], by[i]);
                        break;
                    case "braises":
                        by[i] -= bv[i];
                        bx[i] += (float)Math.Sin(tick * 0.05 + bp[i]) * 0.6f;
                        if (by[i] < -10) { by[i] = H + 10; bx[i] = rnd.Next(W); }
                        int fa = (int)(ba[i] * (0.6 + 0.4 * Math.Sin(tick * 0.2 + bp[i])));
                        Color ember = Mix(acc, Color.White, 0.35);
                        using (SolidBrush glow = new SolidBrush(Color.FromArgb(fa / 4, ember)))
                            g.FillEllipse(glow, bx[i] - br[i] * 2.5f, by[i] - br[i] * 2.5f, br[i] * 5, br[i] * 5);
                        using (SolidBrush core = new SolidBrush(Color.FromArgb(fa, ember)))
                            g.FillEllipse(core, bx[i] - br[i], by[i] - br[i], br[i] * 2, br[i] * 2);
                        break;
                    case "lucioles":
                        bp[i] += ((float)rnd.NextDouble() - 0.5f) * 0.15f;
                        bx[i] += (float)Math.Cos(bp[i]) * bv[i];
                        by[i] += (float)Math.Sin(bp[i]) * bv[i];
                        if (bx[i] < -10) bx[i] = W + 10; else if (bx[i] > W + 10) bx[i] = -10;
                        if (by[i] < -10) by[i] = H + 10; else if (by[i] > H + 10) by[i] = -10;
                        double pulse = 0.5 + 0.5 * Math.Sin(tick * 0.05 + i);
                        Color fly = Mix(acc, Color.White, 0.5);
                        using (SolidBrush g1 = new SolidBrush(Color.FromArgb((int)(pulse * 25), fly)))
                            g.FillEllipse(g1, bx[i] - br[i] * 4, by[i] - br[i] * 4, br[i] * 8, br[i] * 8);
                        using (SolidBrush g2 = new SolidBrush(Color.FromArgb((int)(pulse * 60), fly)))
                            g.FillEllipse(g2, bx[i] - br[i] * 2.2f, by[i] - br[i] * 2.2f, br[i] * 4.4f, br[i] * 4.4f);
                        using (SolidBrush g3 = new SolidBrush(Color.FromArgb((int)(60 + pulse * 190), fly)))
                            g.FillEllipse(g3, bx[i] - br[i] * 0.7f, by[i] - br[i] * 0.7f, br[i] * 1.4f, br[i] * 1.4f);
                        break;
                    default:
                        bx[i] += (float)Math.Cos(bp[i] + tick * 0.003) * bv[i] * 0.4f;
                        by[i] -= bv[i] * 0.3f;
                        if (by[i] < -br[i] * 2) { by[i] = H + br[i] * 2; bx[i] = rnd.Next(W); }
                        using (SolidBrush bs = new SolidBrush(Color.FromArgb(ba[i], Mix(acc, Color.White, 0.2))))
                            g.FillEllipse(bs, bx[i] - br[i], by[i] - br[i], br[i] * 2, br[i] * 2);
                        using (Pen rp = new Pen(Color.FromArgb(Math.Min(255, ba[i] + 30), acc), 1f))
                            g.DrawEllipse(rp, bx[i] - br[i], by[i] - br[i], br[i] * 2, br[i] * 2);
                        break;
                }
            }
        }

        static void RenderWaves(Graphics g)
        {
            Color acc = Theme.AccentC;
            using (LinearGradientBrush lg = new LinearGradientBrush(new Rectangle(0, 0, W, H), Mix(Theme.BgC, Color.Black, 0.25), Mix(Theme.BgC, acc, 0.18), 90f))
                g.FillRectangle(lg, 0, 0, W, H);
            for (int w = 0; w < 4; w++)
            {
                List<PointF> pts = new List<PointF>();
                float baseY = H * (0.55f + w * 0.1f);
                double speed = 0.03 * (1 + w * 0.3);
                for (int x = 0; x <= W + 10; x += 10)
                    pts.Add(new PointF(x, baseY + (float)(Math.Sin(x * 0.009 + tick * speed + w * 1.7) * (16 + w * 5))));
                pts.Add(new PointF(W + 10, H + 5));
                pts.Add(new PointF(0, H + 5));
                using (SolidBrush sb = new SolidBrush(Color.FromArgb(50 + w * 6, Mix(acc, Color.White, w * 0.08))))
                    g.FillPolygon(sb, pts.ToArray());
            }
        }

        static void RenderGrid(Graphics g)
        {
            Color acc = Theme.AccentC;
            using (LinearGradientBrush lg = new LinearGradientBrush(new Rectangle(0, 0, W, H), Mix(Theme.BgC, Color.Black, 0.4), Mix(Theme.BgC, acc, 0.25), 90f))
                g.FillRectangle(lg, 0, 0, W, H);
            float hy = H * 0.5f;
            using (Pen p = new Pen(Color.FromArgb(120, acc), 1.2f))
            {
                double off = (tick * 0.015) % 1.0;
                for (int i = 0; i < 16; i++)
                {
                    double t = (i + off) / 16.0;
                    float y = hy + (H - hy) * (float)(t * t);
                    g.DrawLine(p, 0, y, W, y);
                }
                for (int k = -14; k <= 14; k++)
                    g.DrawLine(p, W / 2f + k * W * 0.02f, hy, W / 2f + k * W * 0.16f, H);
            }
            using (Pen hp = new Pen(Color.FromArgb(200, Mix(acc, Color.White, 0.4)), 2f))
                g.DrawLine(hp, 0, hy, W, hy);
        }

        static void RenderGradient(Graphics g)
        {
            using (LinearGradientBrush lg = new LinearGradientBrush(new Rectangle(0, 0, W, H),
                Mix(Theme.BgC, Color.Black, 0.35), Mix(Theme.BgC, Theme.AccentC, 0.5), (float)((tick * 0.4) % 360)))
            {
                lg.SetBlendTriangularShape(0.5f);
                g.FillRectangle(lg, 0, 0, W, H);
            }
        }

        static void RenderRipples(Graphics g)
        {
            Color acc = Theme.AccentC;
            using (LinearGradientBrush lg = new LinearGradientBrush(new Rectangle(0, 0, W, H), Mix(Theme.BgC, Color.Black, 0.3), Mix(Theme.BgC, acc, 0.12), 90f))
                g.FillRectangle(lg, 0, 0, W, H);
            if (!rInit)
            {
                for (int i = 0; i < 5; i++)
                {
                    rx[i] = rnd.Next(W);
                    ry[i] = rnd.Next(H);
                    rs[i] = tick - i * 50;
                }
                rInit = true;
            }
            for (int i = 0; i < 5; i++)
            {
                float rad = (tick - rs[i]) * 1.8f;
                if (rad > 380) { rs[i] = tick; rx[i] = rnd.Next(W); ry[i] = rnd.Next(H); rad = 0; }
                if (rad < 0) continue;
                double fade = 1 - rad / 380.0;
                for (int k = 0; k < 3; k++)
                {
                    float rr = rad - k * 18;
                    if (rr <= 0) continue;
                    int al = (int)(170 * fade * (1 - k * 0.3));
                    if (al <= 0) continue;
                    using (Pen p = new Pen(Color.FromArgb(Math.Min(255, al), acc), 1.6f))
                        g.DrawEllipse(p, rx[i] - rr, ry[i] - rr, rr * 2, rr * 2);
                }
            }
        }

        public static void Draw(Graphics g, Rectangle clip)
        {
            Rectangle fb = frame == null ? Rectangle.Empty : new Rectangle(0, 0, frame.Width, frame.Height);
            if (frame == null || !fb.Contains(clip))
                using (SolidBrush b = new SolidBrush(Theme.BgC)) g.FillRectangle(b, clip);
            if (frame == null) return;
            Rectangle r = Rectangle.Intersect(clip, fb);
            if (r.IsEmpty) return;
            g.DrawImage(frame, r, r.X, r.Y, r.Width, r.Height, GraphicsUnit.Pixel);
        }

        static void RenderImage(Graphics g)
        {
            g.Clear(Theme.BgC);
            if (img == null) return;
            double sc = Math.Max((double)W / img.Width, (double)H / img.Height);
            int dw = (int)Math.Ceiling(img.Width * sc);
            int dh = (int)Math.Ceiling(img.Height * sc);
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;
            g.DrawImage(img, (W - dw) / 2, (H - dh) / 2, dw, dh);
            using (SolidBrush b = new SolidBrush(Color.FromArgb(150, Theme.BgC)))
                g.FillRectangle(b, 0, 0, W, H);
        }

        static void RenderBubbles(Graphics g)
        {
            Color acc = Theme.AccentC;
            using (LinearGradientBrush lg = new LinearGradientBrush(new Rectangle(0, 0, W, H), Theme.BgC, Mix(Theme.BgC, acc, 0.22), 90f))
                g.FillRectangle(lg, 0, 0, W, H);
            if (!bInit)
            {
                for (int i = 0; i < BUB; i++)
                {
                    bx[i] = rnd.Next(W);
                    by[i] = rnd.Next(H);
                    br[i] = 3 + (float)rnd.NextDouble() * 10;
                    bv[i] = 0.3f + (float)rnd.NextDouble() * 1.1f;
                    bp[i] = (float)(rnd.NextDouble() * 6.28);
                    ba[i] = 25 + rnd.Next(80);
                }
                bInit = true;
            }
            for (int i = 0; i < BUB; i++)
            {
                by[i] -= bv[i];
                bx[i] += (float)Math.Sin(tick * 0.03 + bp[i]) * 0.4f;
                if (by[i] < -20) { by[i] = H + 20; bx[i] = rnd.Next(W); }
                using (SolidBrush sb = new SolidBrush(Color.FromArgb(ba[i], acc)))
                    g.FillEllipse(sb, bx[i] - br[i], by[i] - br[i], br[i] * 2, br[i] * 2);
                using (Pen p = new Pen(Color.FromArgb(Math.Min(255, ba[i] + 60), acc), 1f))
                    g.DrawEllipse(p, bx[i] - br[i], by[i] - br[i], br[i] * 2, br[i] * 2);
            }
        }

        static void RenderAurora(Graphics g)
        {
            // dessine en demi-resolution (plus rapide), puis agrandit : les degrades restent doux
            using (Graphics sg = Graphics.FromImage(small))
            {
                sg.SmoothingMode = SmoothingMode.AntiAlias;
                sg.Clear(Theme.BgC);
                int sw = small.Width, sh = small.Height;
                double t = tick * 0.012;
                float rad = Math.Max(sw, sh) * 0.55f;
                for (int i = 0; i < 3; i++)
                {
                    float cx = (float)(sw * (0.5 + 0.42 * Math.Sin(t * (0.7 + i * 0.35) + i * 2.1)));
                    float cy = (float)(sh * (0.5 + 0.38 * Math.Cos(t * (0.55 + i * 0.3) + i * 1.3)));
                    Color col = i == 1 ? Mix(Theme.AccentC, Color.White, 0.25) : Theme.AccentC;
                    RectangleF rc = new RectangleF(cx - rad, cy - rad, rad * 2, rad * 2);
                    using (GraphicsPath gp = new GraphicsPath())
                    {
                        gp.AddEllipse(rc);
                        using (PathGradientBrush pg = new PathGradientBrush(gp))
                        {
                            pg.CenterColor = Color.FromArgb(95, col);
                            pg.SurroundColors = new Color[] { Color.FromArgb(0, col) };
                            sg.FillEllipse(pg, rc);
                        }
                    }
                }
            }
            g.InterpolationMode = InterpolationMode.Bilinear;
            g.DrawImage(small, 0, 0, W, H);
        }

        static void RenderMatrix(Graphics g)
        {
            g.Clear(Mix(Theme.BgC, Color.Black, 0.45));
            if (mFont == null) mFont = new Font("Consolas", 11f, FontStyle.Bold);
            float lh = mFont.GetHeight(g);
            int rows = (int)(H / lh) + 2;
            int cols = W / CW + 1;
            if (my == null || mCols != cols)
            {
                mCols = cols;
                my = new float[cols]; mv = new float[cols]; ml = new int[cols]; mc = new char[cols][];
                for (int c = 0; c < cols; c++)
                {
                    my[c] = -rnd.Next(0, rows);
                    mv[c] = 0.25f + (float)rnd.NextDouble() * 0.45f;
                    ml[c] = 6 + rnd.Next(14);
                    mc[c] = new char[40];
                    for (int k = 0; k < 40; k++) mc[c][k] = Glyphs[rnd.Next(Glyphs.Length)];
                }
            }
            Color trail = Color.FromArgb(120, Theme.AccentC);
            Color head = Color.FromArgb(235, Mix(Theme.AccentC, Color.White, 0.65));
            using (SolidBrush tb = new SolidBrush(trail))
            using (SolidBrush hb = new SolidBrush(head))
            {
                StringBuilder sb = new StringBuilder();
                for (int c = 0; c < cols; c++)
                {
                    my[c] += mv[c];
                    if (my[c] - ml[c] > rows)
                    {
                        my[c] = -rnd.Next(2, 20);
                        mv[c] = 0.25f + (float)rnd.NextDouble() * 0.45f;
                        ml[c] = 6 + rnd.Next(14);
                    }
                    mc[c][rnd.Next(40)] = Glyphs[rnd.Next(Glyphs.Length)];
                    int headRow = (int)my[c];
                    if (headRow < 0) continue;
                    int first = headRow - ml[c] + 1;
                    sb.Length = 0;
                    for (int r = first; r < headRow; r++)
                    {
                        sb.Append(mc[c][((r % 40) + 40) % 40]);
                        sb.Append('\n');
                    }
                    if (sb.Length > 0) g.DrawString(sb.ToString(), mFont, tb, c * CW, first * lh);
                    g.DrawString(mc[c][headRow % 40].ToString(), mFont, hb, c * CW, headRow * lh);
                }
            }
        }
    }

    // ---------- Chiffrement : AES-256-CBC + HMAC-SHA256, cle derivee par PBKDF2 (200 000 iterations) ----------
    static class Vault
    {
        public static byte[] encKey, macKey, salt;
        public static List<TrashItem> Trash = new List<TrashItem>();

        public static string DataDir
        {
            get { return System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MonCoffre"); }
        }
        public static string DataFile
        {
            get { return System.IO.Path.Combine(DataDir, "coffre.dat"); }
        }

        static void DeriveKeys(string pw, byte[] s, out byte[] ek, out byte[] mk)
        {
            using (Rfc2898DeriveBytes kdf = new Rfc2898DeriveBytes(pw, s, 200000, HashAlgorithmName.SHA256))
            {
                byte[] k = kdf.GetBytes(64);
                ek = new byte[32]; mk = new byte[32];
                Array.Copy(k, 0, ek, 0, 32);
                Array.Copy(k, 32, mk, 0, 32);
            }
        }

        public static void NewKeys(string pw)
        {
            byte[] s = new byte[16];
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider()) rng.GetBytes(s);
            byte[] ek, mk;
            DeriveKeys(pw, s, out ek, out mk);
            salt = s; encKey = ek; macKey = mk;
        }

        public static void Wipe() { encKey = null; macKey = null; salt = null; Trash = new List<TrashItem>(); }

        static void WriteRow(BinaryWriter w, Row r)
        {
            w.Write(r.Label ?? "");
            w.Write(r.Value ?? "");
            w.Write(r.Secret);
        }

        static Row ReadRow(BinaryReader rd)
        {
            Row r = new Row();
            r.Label = rd.ReadString();
            r.Value = rd.ReadString();
            r.Secret = rd.ReadBoolean();
            return r;
        }

        static void WriteBox(BinaryWriter w, Box b)
        {
            w.Write(b.Title ?? "");
            w.Write(b.Icon ?? "");
            w.Write(b.Rows.Count);
            foreach (Row r in b.Rows) WriteRow(w, r);
        }

        static Box ReadBox(BinaryReader rd, int ver)
        {
            Box b = new Box();
            b.Title = rd.ReadString();
            if (ver >= 2) b.Icon = rd.ReadString();
            int m = rd.ReadInt32();
            for (int j = 0; j < m; j++) b.Rows.Add(ReadRow(rd));
            return b;
        }

        static byte[] Serialize(List<Box> boxes)
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter w = new BinaryWriter(ms, Encoding.UTF8))
            {
                w.Write(2);
                w.Write(boxes.Count);
                foreach (Box b in boxes) WriteBox(w, b);
                w.Write(Trash.Count);
                foreach (TrashItem t in Trash)
                {
                    w.Write(t.IsRow);
                    w.Write(t.Ticks);
                    w.Write(t.ParentTitle ?? "");
                    if (t.IsRow) WriteRow(w, t.R); else WriteBox(w, t.B);
                }
                w.Flush();
                return ms.ToArray();
            }
        }

        static List<Box> Deserialize(byte[] data, out List<TrashItem> trash)
        {
            List<Box> list = new List<Box>();
            trash = new List<TrashItem>();
            using (MemoryStream ms = new MemoryStream(data))
            using (BinaryReader rd = new BinaryReader(ms, Encoding.UTF8))
            {
                int ver = rd.ReadInt32();
                if (ver != 1 && ver != 2) throw new InvalidDataException("version inconnue");
                int n = rd.ReadInt32();
                for (int i = 0; i < n; i++) list.Add(ReadBox(rd, ver));
                if (ver >= 2)
                {
                    int tn = rd.ReadInt32();
                    for (int i = 0; i < tn; i++)
                    {
                        TrashItem t = new TrashItem();
                        t.IsRow = rd.ReadBoolean();
                        t.Ticks = rd.ReadInt64();
                        t.ParentTitle = rd.ReadString();
                        if (t.IsRow) t.R = ReadRow(rd); else t.B = ReadBox(rd, ver);
                        trash.Add(t);
                    }
                }
            }
            return list;
        }

        public static void Save(List<Box> boxes)
        {
            byte[] pt = Serialize(boxes);
            byte[] iv = new byte[16];
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider()) rng.GetBytes(iv);
            byte[] ct;
            using (Aes aes = Aes.Create())
            {
                aes.Key = encKey; aes.IV = iv; aes.Mode = CipherMode.CBC; aes.Padding = PaddingMode.PKCS7;
                using (ICryptoTransform t = aes.CreateEncryptor()) ct = t.TransformFinalBlock(pt, 0, pt.Length);
            }
            byte[] all = new byte[32 + ct.Length + 32];
            Array.Copy(salt, 0, all, 0, 16);
            Array.Copy(iv, 0, all, 16, 16);
            Array.Copy(ct, 0, all, 32, ct.Length);
            byte[] mac;
            using (HMACSHA256 h = new HMACSHA256(macKey)) mac = h.ComputeHash(all, 0, 32 + ct.Length);
            Array.Copy(mac, 0, all, 32 + ct.Length, 32);

            Directory.CreateDirectory(DataDir);
            string tmp = DataFile + ".tmp";
            File.WriteAllBytes(tmp, all);
            File.Copy(tmp, DataFile, true);
            File.Delete(tmp);
        }

        // Ouvre un fichier chiffre. Ne change les cles en memoire qu'en cas de succes.
        public static List<Box> Open(byte[] file, string pw)
        {
            if (file.Length < 32 + 16 + 32) throw new CryptographicException("fichier invalide");
            byte[] s = new byte[16];
            Array.Copy(file, 0, s, 0, 16);
            byte[] ek, mk;
            DeriveKeys(pw, s, out ek, out mk);
            int macPos = file.Length - 32;
            byte[] mac;
            using (HMACSHA256 h = new HMACSHA256(mk)) mac = h.ComputeHash(file, 0, macPos);
            int diff = 0;
            for (int i = 0; i < 32; i++) diff |= mac[i] ^ file[macPos + i];
            if (diff != 0) throw new CryptographicException("mot de passe incorrect");
            byte[] iv = new byte[16];
            Array.Copy(file, 16, iv, 0, 16);
            byte[] pt;
            using (Aes aes = Aes.Create())
            {
                aes.Key = ek; aes.IV = iv; aes.Mode = CipherMode.CBC; aes.Padding = PaddingMode.PKCS7;
                using (ICryptoTransform t = aes.CreateDecryptor()) pt = t.TransformFinalBlock(file, 32, macPos - 32);
            }
            List<TrashItem> tr;
            List<Box> boxes = Deserialize(pt, out tr);
            encKey = ek; macKey = mk; salt = s;
            Trash = tr;
            return boxes;
        }
    }

    // ---------- Blocage apres 3 erreurs (5, 10, 15... minutes), conserve meme si on ferme l'appli ----------
    static class Lockout
    {
        public static int Fails;          // erreurs depuis le dernier blocage (0 a 2)
        public static int Level;          // nombre de blocages deja subis
        public static long UntilTicks;    // fin du blocage (UTC)

        static readonly byte[] Key = Encoding.UTF8.GetBytes("MonCoffre-lockout-v1-x9Qw2");
        const string RegPath = @"Software\MonCoffre";

        static string FilePath
        {
            get { return System.IO.Path.Combine(Vault.DataDir, "lock.dat"); }
        }

        static string Sign(string s)
        {
            using (HMACSHA256 h = new HMACSHA256(Key))
            {
                byte[] m = h.ComputeHash(Encoding.UTF8.GetBytes(s));
                return BitConverter.ToString(m).Replace("-", "");
            }
        }

        static bool Parse(string raw, out int f, out int l, out long u)
        {
            f = 0; l = 0; u = 0;
            if (string.IsNullOrEmpty(raw)) return false;
            string[] p = raw.Trim().Split('|');
            if (p.Length != 4) return false;
            string body = p[0] + "|" + p[1] + "|" + p[2];
            if (Sign(body) != p[3]) return false;
            return int.TryParse(p[0], out f) && int.TryParse(p[1], out l) && long.TryParse(p[2], out u);
        }

        // Lit l'etat dans le fichier ET dans le registre Windows, garde le plus strict.
        public static void Load()
        {
            string raw1 = null, raw2 = null;
            try { if (File.Exists(FilePath)) raw1 = File.ReadAllText(FilePath); } catch (Exception) { }
            try
            {
                using (Microsoft.Win32.RegistryKey k = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(RegPath))
                    if (k != null) raw2 = k.GetValue("l") as string;
            }
            catch (Exception) { }
            int f1, l1, f2, l2; long u1, u2;
            bool ok1 = Parse(raw1, out f1, out l1, out u1);
            bool ok2 = Parse(raw2, out f2, out l2, out u2);
            Fails = 0; Level = 0; UntilTicks = 0;
            if (ok1 && ok2)
            {
                bool firstStricter = l1 > l2 || (l1 == l2 && (f1 > f2 || (f1 == f2 && u1 >= u2)));
                if (firstStricter) { Fails = f1; Level = l1; UntilTicks = u1; }
                else { Fails = f2; Level = l2; UntilTicks = u2; }
            }
            else if (ok1) { Fails = f1; Level = l1; UntilTicks = u1; }
            else if (ok2) { Fails = f2; Level = l2; UntilTicks = u2; }
        }

        public static void Save()
        {
            string body = Fails + "|" + Level + "|" + UntilTicks;
            string val = body + "|" + Sign(body);
            try { Directory.CreateDirectory(Vault.DataDir); File.WriteAllText(FilePath, val); } catch (Exception) { }
            try
            {
                using (Microsoft.Win32.RegistryKey k = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(RegPath))
                    k.SetValue("l", val);
            }
            catch (Exception) { }
        }

        public static void Reset()
        {
            Fails = 0; Level = 0; UntilTicks = 0;
            try { if (File.Exists(FilePath)) File.Delete(FilePath); } catch (Exception) { }
            try
            {
                using (Microsoft.Win32.RegistryKey k = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(RegPath, true))
                    if (k != null) k.DeleteValue("l", false);
            }
            catch (Exception) { }
        }

        public static int RemainingSeconds()
        {
            if (UntilTicks <= 0) return 0;
            double s = (new DateTime(UntilTicks, DateTimeKind.Utc) - DateTime.UtcNow).TotalSeconds;
            double cap = Math.Max(1, Level) * 300.0;   // jamais plus que la duree normale du blocage
            if (s > cap) s = cap;
            return s > 0 ? (int)Math.Ceiling(s) : 0;
        }

        // Enregistre une erreur. Renvoie true si cette erreur declenche un blocage.
        public static bool RegisterFail()
        {
            Fails++;
            bool locked = false;
            if (Fails >= 3)
            {
                Level++;
                Fails = 0;
                UntilTicks = DateTime.UtcNow.AddMinutes(5.0 * Level).Ticks;
                locked = true;
            }
            Save();
            return locked;
        }
    }

    // ---------- Fenetre du mot de passe maitre, avec minuteur de blocage ----------
    static class UnlockDialog
    {
        public static bool Show(Func<string, bool> tryPw)
        {
            using (Form f = new Form())
            {
                f.Text = "Mon Coffre";
                f.FormBorderStyle = FormBorderStyle.FixedDialog;
                f.StartPosition = FormStartPosition.CenterScreen;
                f.MaximizeBox = false; f.MinimizeBox = false;
                f.ShowInTaskbar = true; f.TopMost = true;
                f.Font = new Font("Segoe UI", 10f);
                f.ClientSize = new Size(440, 218);

                Label msg = new Label();
                msg.Text = "Entre ton mot de passe maître.";
                msg.SetBounds(14, 14, 412, 24);

                Label timer = new Label();
                timer.SetBounds(14, 44, 412, 46);
                timer.TextAlign = ContentAlignment.MiddleCenter;
                timer.Font = new Font("Segoe UI", 24f, FontStyle.Bold);

                Label lb = new Label();
                lb.Text = "Mot de passe";
                lb.SetBounds(14, 107, 140, 24);

                TextBox tb = new TextBox();
                tb.SetBounds(160, 104, 266, 26);
                tb.UseSystemPasswordChar = true;

                Label err = new Label();
                err.SetBounds(14, 140, 412, 40);

                Button ok = new Button();
                ok.Text = "OK";
                ok.SetBounds(236, 180, 90, 30);
                Button no = new Button();
                no.Text = "Annuler";
                no.DialogResult = DialogResult.Cancel;
                no.SetBounds(336, 180, 90, 30);

                f.Controls.Add(msg); f.Controls.Add(timer); f.Controls.Add(lb);
                f.Controls.Add(tb); f.Controls.Add(err); f.Controls.Add(ok); f.Controls.Add(no);
                f.AcceptButton = ok; f.CancelButton = no;
                f.ActiveControl = tb;

                Theme.Apply(f);
                Theme.DarkTitle(f);
                ok.BackColor = Theme.AccentC;
                ok.FlatAppearance.BorderColor = Theme.AccentC;
                timer.ForeColor = Theme.IsLight ? Color.Black : Color.White;
                err.ForeColor = Color.FromArgb(255, 110, 110);

                bool wasLocked = false;
                Timer t = new Timer();
                t.Interval = 500;
                EventHandler refresh = (s, e) =>
                {
                    int rem = Lockout.RemainingSeconds();
                    if (rem > 0)
                    {
                        wasLocked = true;
                        timer.Text = string.Format("{0:00}:{1:00}", rem / 60, rem % 60);
                        tb.Text = "";
                        tb.Enabled = false;
                        ok.Enabled = false;
                        if (err.Text.Length == 0) err.Text = "Trop d'erreurs : accès bloqué.";
                    }
                    else
                    {
                        timer.Text = "";
                        if (!tb.Enabled) { tb.Enabled = true; ok.Enabled = true; tb.Focus(); }
                        if (wasLocked) { wasLocked = false; err.Text = "Tu peux réessayer."; }
                    }
                };
                t.Tick += refresh;

                ok.Click += (s, e) =>
                {
                    if (Lockout.RemainingSeconds() > 0) return;
                    string pw = tb.Text;
                    if (pw.Length == 0) return;
                    ok.Enabled = false;
                    f.Cursor = Cursors.WaitCursor;
                    bool good;
                    try { good = tryPw(pw); }
                    finally { f.Cursor = Cursors.Default; }
                    tb.Text = "";
                    if (good)
                    {
                        Lockout.Reset();
                        f.DialogResult = DialogResult.OK;
                        return;
                    }
                    bool locked = Lockout.RegisterFail();
                    if (locked) err.Text = "Trop d'erreurs : accès bloqué.";
                    else err.Text = "Mot de passe incorrect. Essais restants avant blocage : " + (3 - Lockout.Fails) + ".";
                    ok.Enabled = true;
                    refresh(null, null);
                    tb.Focus();
                };

                t.Start();
                refresh(null, null);
                DialogResult res = f.ShowDialog();
                t.Stop();
                t.Dispose();
                return res == DialogResult.OK;
            }
        }
    }

    // ---------- Petite fenetre de saisie ----------
    static class Prompt
    {
        public static string[] Show(string title, string msg, string[] labels, string[] initial, bool pw)
        {
            using (Form f = new Form())
            {
                f.Text = title;
                f.FormBorderStyle = FormBorderStyle.FixedDialog;
                f.StartPosition = FormStartPosition.CenterScreen;
                f.MaximizeBox = false; f.MinimizeBox = false;
                f.ShowInTaskbar = true; f.TopMost = true;
                f.Font = new Font("Segoe UI", 10f);
                int n = labels.Length;
                f.ClientSize = new Size(440, 84 + n * 36 + 52);

                Label l = new Label();
                l.Text = msg;
                l.SetBounds(14, 12, 412, 64);
                f.Controls.Add(l);

                TextBox[] tbs = new TextBox[n];
                for (int i = 0; i < n; i++)
                {
                    int y = 84 + i * 36;
                    Label lb = new Label();
                    lb.Text = labels[i];
                    lb.SetBounds(14, y + 3, 140, 24);
                    f.Controls.Add(lb);
                    TextBox tb = new TextBox();
                    tb.SetBounds(160, y, 266, 26);
                    tb.UseSystemPasswordChar = pw;
                    if (initial != null && i < initial.Length) tb.Text = initial[i];
                    f.Controls.Add(tb);
                    tbs[i] = tb;
                }
                Button ok = new Button();
                ok.Text = "OK"; ok.DialogResult = DialogResult.OK;
                ok.SetBounds(236, 84 + n * 36 + 8, 90, 30);
                Button no = new Button();
                no.Text = "Annuler"; no.DialogResult = DialogResult.Cancel;
                no.SetBounds(336, 84 + n * 36 + 8, 90, 30);
                f.Controls.Add(ok); f.Controls.Add(no);
                f.AcceptButton = ok; f.CancelButton = no;
                f.ActiveControl = tbs[0];
                Theme.Apply(f);
                Theme.DarkTitle(f);
                ok.BackColor = Theme.AccentC;
                ok.FlatAppearance.BorderColor = Theme.AccentC;

                if (f.ShowDialog() != DialogResult.OK) return null;
                string[] res = new string[n];
                for (int i = 0; i < n; i++) res[i] = tbs[i].Text;
                return res;
            }
        }
    }

    // ---------- Fenetre principale ----------
    class MainForm : Form
    {
        List<Box> boxes = new List<Box>();
        Box shown;
        bool loading;
        string lastCopied;
        public DateTime lastAct = DateTime.Now;

        ListBox lst = new ListBox();
        TextBox search = new TextBox();
        DataGridView grid = new DataGridView();
        Label lblInfo = new Label();
        Timer clipTimer = new Timer();
        Timer idleTimer = new Timer();
        Timer infoTimer = new Timer();
        Timer animTimer = new Timer();
        Timer clockTimer = new Timer();
        Label clockLbl = new Label();
        StrengthBar strBar = new StrengthBar();
        Label lblStr = new Label();
        List<Control> frames = new List<Control>();
        Font iconFont = new Font("Segoe UI Emoji", 12f);
        Panel veil = new Panel();
        Timer veilTimer = new Timer();
        DateTime lastEsc = DateTime.MinValue;

        class ActivityFilter : IMessageFilter
        {
            MainForm f;
            public ActivityFilter(MainForm form) { f = form; }
            public bool PreFilterMessage(ref Message m)
            {
                if (m.Msg == 0x100 || m.Msg == 0x200 || m.Msg == 0x201 || m.Msg == 0x20A) f.lastAct = DateTime.Now;
                return false;
            }
        }

        static Button Btn(string text, EventHandler h)
        {
            Button b = new Button();
            b.Text = text;
            b.AutoSize = true;
            b.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            b.Padding = new Padding(6, 2, 6, 2);
            b.Margin = new Padding(4);
            b.Click += h;
            return b;
        }

        static string S(object o) { return o == null ? "" : o.ToString(); }

        static bool IsSecret(DataGridViewRow r)
        {
            object v = r.Cells[2].Value;
            return v is bool && (bool)v;
        }

        public MainForm()
        {
            Text = "Mon Coffre";
            Font = new Font("Segoe UI", 10f);
            DoubleBuffered = true;
            ClientSize = new Size(1240, 700);
            MinimumSize = new Size(760, 420);
            StartPosition = FormStartPosition.CenterScreen;

            // ----- partie droite : tableau -----
            Panel right = new Panel();
            right.Dock = DockStyle.Fill;
            right.Padding = new Padding(0, 0, 14, 12);

            grid.Dock = DockStyle.Fill;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = true;
            grid.RowHeadersVisible = false;
            grid.BackgroundColor = Color.White;
            grid.BorderStyle = BorderStyle.FixedSingle;
            grid.MultiSelect = false;
            grid.EditMode = DataGridViewEditMode.EditOnEnter;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.RowTemplate.Height = 32;
            DataGridViewTextBoxColumn c0 = new DataGridViewTextBoxColumn();
            c0.HeaderText = "Nom"; c0.FillWeight = 30;
            DataGridViewTextBoxColumn c1 = new DataGridViewTextBoxColumn();
            c1.HeaderText = "Valeur"; c1.FillWeight = 60;
            DataGridViewButtonColumn c2 = new DataGridViewButtonColumn();
            c2.HeaderText = "Copier"; c2.FillWeight = 12; c2.MinimumWidth = 70;
            c2.SortMode = DataGridViewColumnSortMode.NotSortable;
            c2.Resizable = DataGridViewTriState.False;
            c2.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grid.Columns.Add(c0); grid.Columns.Add(c1); grid.Columns.Add(c2);

            grid.CellValueChanged += (s, e) =>
            {
                if (loading) return;
                Sync();
                if (grid.CurrentCell != null && e.RowIndex == grid.CurrentCell.RowIndex) UpdateStrength(CurrentValue());
            };
            grid.CurrentCellChanged += (s, e) => { if (!loading) UpdateStrength(CurrentValue()); };
            grid.MouseUp += (s, e) =>
            {
                if (e.Button != MouseButtons.Left) return;
                DataGridView.HitTestInfo hit = grid.HitTest(e.X, e.Y);
                if (hit.Type != DataGridViewHitTestType.Cell || hit.ColumnIndex != 2 || hit.RowIndex < 0) return;
                CopyRow(hit.RowIndex);
            };
            grid.CellPainting += (s, e) =>
            {
                if (e.RowIndex < 0 || e.ColumnIndex != 2) return;
                e.Paint(e.CellBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border);
                DataGridViewRow row = grid.Rows[e.RowIndex];
                if (!row.IsNewRow)
                {
                    bool sel = (e.State & DataGridViewElementStates.Selected) != 0;
                    Color bgc = sel ? e.CellStyle.SelectionBackColor : e.CellStyle.BackColor;
                    int cx = e.CellBounds.X + e.CellBounds.Width / 2;
                    int cy = e.CellBounds.Y + e.CellBounds.Height / 2;
                    System.Drawing.Drawing2D.SmoothingMode old = e.Graphics.SmoothingMode;
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    using (Pen p = new Pen(Theme.AccentC, 1.8f))
                    using (SolidBrush fill = new SolidBrush(bgc))
                    {
                        Rectangle back = new Rectangle(cx - 8, cy - 9, 10, 13);
                        Rectangle front = new Rectangle(cx - 3, cy - 4, 10, 13);
                        e.Graphics.DrawRectangle(p, back);
                        e.Graphics.FillRectangle(fill, front);
                        e.Graphics.DrawRectangle(p, front);
                    }
                    e.Graphics.SmoothingMode = old;
                }
                e.Handled = true;
            };
            grid.CellMouseEnter += (s, e) =>
            {
                if (e.ColumnIndex == 2 && e.RowIndex >= 0 && !grid.Rows[e.RowIndex].IsNewRow) grid.Cursor = Cursors.Hand;
            };
            grid.CellMouseLeave += (s, e) => { grid.Cursor = Cursors.Default; };
            grid.UserDeletedRow += (s, e) => { if (!loading) Sync(); };
            grid.DataError += (s, e) => { e.ThrowException = false; };
            grid.EditingControlShowing += (s, e) =>
            {
                TextBox tb = e.Control as TextBox;
                if (tb == null || grid.CurrentCell == null) return;
                tb.UseSystemPasswordChar = false;
                tb.BackColor = Theme.PanelC;
                tb.ForeColor = Theme.TextC;
                tb.TextChanged -= EditTextChanged;
                tb.TextChanged += EditTextChanged;
            };

            FlowLayoutPanel bottom = new FlowLayoutPanel();
            bottom.Dock = DockStyle.Bottom;
            bottom.AutoSize = true;
            bottom.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            bottom.Padding = new Padding(0, 6, 0, 0);
            lblInfo.AutoSize = true;
            lblInfo.ForeColor = Color.SeaGreen;
            lblInfo.Margin = new Padding(10, 10, 0, 0);
            bottom.Controls.Add(Btn("+ Créer une ligne", (s, e) => AddRow()));
            bottom.Controls.Add(Btn("▲ Monter la ligne", (s, e) => MoveRow(-1)));
            bottom.Controls.Add(Btn("▼ Descendre la ligne", (s, e) => MoveRow(1)));
            bottom.Controls.Add(Btn("✕ Supprimer la ligne", (s, e) => DeleteRow()));
            bottom.Controls.Add(Btn("Générer un mot de passe", (s, e) => Generate()));
            bottom.Controls.Add(lblInfo);

            FlowLayoutPanel strengthPanel = new FlowLayoutPanel();
            strengthPanel.Dock = DockStyle.Bottom;
            strengthPanel.AutoSize = true;
            strengthPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Label lblStrT = new Label();
            lblStrT.Text = "Force de la valeur sélectionnée :";
            lblStrT.AutoSize = true;
            lblStrT.Margin = new Padding(6, 8, 8, 0);
            strBar.Margin = new Padding(0, 10, 10, 0);
            lblStr.AutoSize = true;
            lblStr.Text = "—";
            lblStr.Margin = new Padding(0, 8, 0, 0);
            strengthPanel.Controls.Add(lblStrT);
            strengthPanel.Controls.Add(strBar);
            strengthPanel.Controls.Add(lblStr);

            right.Controls.Add(grid);
            right.Controls.Add(strengthPanel);
            right.Controls.Add(bottom);

            // ----- partie gauche : liste des cases -----
            Panel left = new Panel();
            left.Dock = DockStyle.Left;
            left.Width = 330;
            left.Padding = new Padding(14, 0, 10, 12);

            Label lblS = new Label();
            lblS.Text = "Rechercher :";
            lblS.Dock = DockStyle.Top;
            lblS.Height = 22;
            search.Dock = DockStyle.Top;
            search.TextChanged += (s, e) => RefreshList(shown);

            lst.Dock = DockStyle.Fill;
            lst.IntegralHeight = false;
            lst.Font = new Font("Segoe UI", 11f);
            lst.ItemHeight = 30;
            lst.DrawMode = DrawMode.OwnerDrawFixed;
            lst.DrawItem += (s, e) =>
            {
                if (e.Index < 0) return;
                bool sel = (e.State & DrawItemState.Selected) != 0;
                using (SolidBrush bg = new SolidBrush(sel ? Theme.SelectC : Theme.PanelC))
                    e.Graphics.FillRectangle(bg, e.Bounds);
                if (sel)
                    using (SolidBrush bar = new SolidBrush(Theme.AccentC))
                        e.Graphics.FillRectangle(bar, e.Bounds.X, e.Bounds.Y, 4, e.Bounds.Height);
                int tx = e.Bounds.X + 12;
                Box item = lst.Items[e.Index] as Box;
                if (item != null && item.Icon.Length > 0)
                {
                    TextRenderer.DrawText(e.Graphics, item.Icon, iconFont,
                        new Rectangle(e.Bounds.X + 8, e.Bounds.Y, 30, e.Bounds.Height),
                        Theme.TextC, TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter);
                    tx = e.Bounds.X + 42;
                }
                TextRenderer.DrawText(e.Graphics, lst.Items[e.Index].ToString(), lst.Font,
                    new Rectangle(tx, e.Bounds.Y, e.Bounds.Right - tx, e.Bounds.Height),
                    Theme.TextC, TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis);
            };
            lst.SelectedIndexChanged += (s, e) =>
            {
                if (loading) return;
                try { grid.EndEdit(); } catch (Exception) { }
                Sync();
                shown = lst.SelectedItem as Box;
                LoadGrid(shown);
            };
            lst.DoubleClick += (s, e) => RenameBox();

            FlowLayoutPanel lbtn = new FlowLayoutPanel();
            lbtn.Dock = DockStyle.Bottom;
            lbtn.AutoSize = true;
            lbtn.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            lbtn.Controls.Add(Btn("+ Ajouter", (s, e) => AddBox()));
            lbtn.Controls.Add(Btn("Renommer", (s, e) => RenameBox()));
            lbtn.Controls.Add(Btn("Icône", (s, e) => ChooseIcon()));
            lbtn.Controls.Add(Btn("Supprimer", (s, e) => DeleteBox()));
            lbtn.Controls.Add(Btn("▲ Monter", (s, e) => MoveBox(-1)));
            lbtn.Controls.Add(Btn("▼ Descendre", (s, e) => MoveBox(1)));

            left.Controls.Add(lst);
            left.Controls.Add(lbtn);
            left.Controls.Add(search);
            left.Controls.Add(lblS);

            // ----- barre du haut -----
            FlowLayoutPanel top = new FlowLayoutPanel();
            top.Dock = DockStyle.Fill;
            top.Padding = new Padding(8, 8, 0, 0);
            top.Controls.Add(Btn("🔒 Verrouiller", (s, e) => LockNow()));
            top.Controls.Add(Btn("🔑 Mot de passe maître", (s, e) => ChangeMaster()));
            top.Controls.Add(Btn("⬇ Sauvegarde", (s, e) => ExportBackup()));
            top.Controls.Add(Btn("⬆ Importer", (s, e) => ImportBackup()));
            top.Controls.Add(Btn("🖼 Fond", (s, e) => ShowBackdropDialog()));
            top.Controls.Add(Btn("🗑 Corbeille", (s, e) => ShowTrash()));
            top.Controls.Add(Btn("🛡 Sécurité", (s, e) => ShowSecurityDialog()));
            top.Controls.Add(Btn("🩺 Audit des mots de passe", (s, e) => ShowAudit()));
            top.Controls.Add(Btn("🛟 Sauvegardes auto", (s, e) => OpenAutoBackups()));
            top.Controls.Add(Btn("📜 Licence", (s, e) => ShowLicenseDialog()));

            Controls.Add(right);
            Controls.Add(left);
            Button styleBtn = Btn("🎨 Changer le style", (s, e) => ShowStyleDialog());
            styleBtn.AutoSize = false;
            styleBtn.Dock = DockStyle.Fill;
            Panel styleHolder = new Panel();
            styleHolder.Dock = DockStyle.Right;
            styleHolder.Width = 200;
            styleHolder.Padding = new Padding(0, 8, 12, 8);
            styleHolder.Controls.Add(styleBtn);
            clockLbl.Dock = DockStyle.Top;
            clockLbl.Height = 30;
            clockLbl.TextAlign = ContentAlignment.MiddleCenter;
            clockLbl.Font = new Font("Segoe UI", 10f);
            clockLbl.Text = DateTime.Now.ToString("ddd d MMM  HH:mm:ss");
            styleHolder.Controls.Add(clockLbl);
            clockTimer.Interval = 1000;
            clockTimer.Tick += (s, e) => { clockLbl.Text = DateTime.Now.ToString("ddd d MMM  HH:mm:ss"); };
            clockTimer.Start();
            Panel topBar = new Panel();
            topBar.Dock = DockStyle.Top;
            topBar.Height = 92;
            topBar.Controls.Add(top);
            topBar.Controls.Add(styleHolder);
            Controls.Add(topBar);

            clipTimer.Interval = 20000;
            clipTimer.Tick += (s, e) => ClearClip();
            infoTimer.Interval = 5000;
            infoTimer.Tick += (s, e) => { infoTimer.Stop(); lblInfo.Text = ""; };
            animTimer.Interval = 50;
            animTimer.Tick += (s, e) =>
            {
                if (!Visible || WindowState == FormWindowState.Minimized || !Backdrop.Animated || veil.Visible) return;
                Backdrop.Render();
                InvalidateBackdrop();
            };
            idleTimer.Interval = 10000;
            idleTimer.Tick += (s, e) =>
            {
                if (Visible && (DateTime.Now - lastAct).TotalMinutes >= 5) LockNow();
            };
            idleTimer.Start();
            Application.AddMessageFilter(new ActivityFilter(this));

            // voile qui cache le contenu quand on quitte la fenetre (option "Sécurité")
            veil.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            veil.Location = new Point(0, 0);
            veil.Size = ClientSize;
            Label veilText = new Label();
            veilText.Dock = DockStyle.Fill;
            veilText.TextAlign = ContentAlignment.MiddleCenter;
            veilText.Font = new Font("Segoe UI", 16f);
            veilText.Text = "🔒  Coffre masqué\nClique dans la fenêtre pour l'afficher";
            veil.Controls.Add(veilText);
            veil.Visible = false;
            Controls.Add(veil);
            veil.BringToFront();
            veilTimer.Interval = 250;
            veilTimer.Tick += (s, e) =>
            {
                veilTimer.Stop();
                if (Prefs.BlurOnLeave && Visible && ActiveForm == null)
                {
                    veil.BringToFront();
                    veil.Visible = true;
                }
            };

            frames.Add(right); frames.Add(left); frames.Add(lbtn); frames.Add(bottom);
            frames.Add(strengthPanel); frames.Add(top); frames.Add(topBar); frames.Add(styleHolder);

            Theme.Apply(this);
            Theme.DarkTitle(this);
            lblInfo.ForeColor = Theme.OkC;
            UpdateStrength("");
            UpdateTitle();
            ApplyBackdrop();
        }

        // ----- connexion / creation du coffre -----
        public bool Unlock()
        {
            shown = null;
            if (!File.Exists(Vault.DataFile))
            {
                while (true)
                {
                    string[] v = Prompt.Show("Créer mon coffre",
                        "Bienvenue ! Choisis un mot de passe maître (8 caractères minimum).\nSi tu l'oublies, tes données sont perdues : il n'existe aucun moyen de le récupérer.",
                        new string[] { "Mot de passe", "Confirmation" }, null, true);
                    if (v == null) return false;
                    if (v[0].Length < 8) { MessageBox.Show("8 caractères minimum.", "Mon Coffre"); continue; }
                    if (v[0] != v[1]) { MessageBox.Show("Les deux mots de passe ne correspondent pas.", "Mon Coffre"); continue; }
                    Vault.NewKeys(v[0]);
                    Vault.Trash = new List<TrashItem>();
                    boxes = new List<Box>();
                    Box b1 = new Box();
                    b1.Title = "Réseaux sociaux";
                    b1.Rows.Add(new Row { Label = "Site", Value = "" });
                    b1.Rows.Add(new Row { Label = "Identifiant", Value = "" });
                    b1.Rows.Add(new Row { Label = "Mot de passe", Value = "", Secret = true });
                    Box b2 = new Box();
                    b2.Title = "Notes";
                    b2.Rows.Add(new Row { Label = "Note", Value = "" });
                    boxes.Add(b1); boxes.Add(b2);
                    Persist();
                    break;
                }
            }
            else
            {
                byte[] data = File.ReadAllBytes(Vault.DataFile);
                Lockout.Load();
                List<Box> opened = null;
                bool okPw = UnlockDialog.Show(pw =>
                {
                    try { opened = Vault.Open(data, pw); return true; }
                    catch (Exception) { return false; }
                });
                if (!okPw) return false;
                boxes = opened;
            }
            PurgeTrash();
            RefreshList(null);
            lastAct = DateTime.Now;
            return true;
        }

        void LockNow()
        {
            try { grid.EndEdit(); } catch (Exception) { }
            Sync();
            loading = true;
            grid.Rows.Clear();
            lst.Items.Clear();
            loading = false;
            boxes = new List<Box>();
            shown = null;
            search.Text = "";
            ClearClip();
            Vault.Wipe();
            Hide();
            if (!Unlock()) { Application.Exit(); return; }
            Show();
            Activate();
        }

        // ----- liste / tableau -----
        static bool Match(Box b, string q)
        {
            if (q.Length == 0) return true;
            if (b.Title.ToLower().Contains(q)) return true;
            foreach (Row r in b.Rows)
            {
                if (r.Label.ToLower().Contains(q)) return true;
                if (r.Value.ToLower().Contains(q)) return true;
            }
            return false;
        }

        void RefreshList(Box select)
        {
            string q = search.Text.Trim().ToLower();
            lst.BeginUpdate();
            lst.Items.Clear();
            foreach (Box b in boxes) if (Match(b, q)) lst.Items.Add(b);
            lst.EndUpdate();
            if (select != null && lst.Items.Contains(select)) lst.SelectedItem = select;
            else if (lst.Items.Count > 0) lst.SelectedIndex = 0;
            else { shown = null; LoadGrid(null); }
        }

        void LoadGrid(Box b)
        {
            loading = true;
            grid.Rows.Clear();
            if (b != null)
                foreach (Row r in b.Rows) grid.Rows.Add(r.Label, r.Value, "");
            grid.Enabled = b != null;
            loading = false;
            UpdateStrength(CurrentValue());
        }

        void Sync()
        {
            if (loading || shown == null) return;
            List<Row> rows = new List<Row>();
            foreach (DataGridViewRow r in grid.Rows)
            {
                if (r.IsNewRow) continue;
                Row x = new Row();
                x.Label = S(r.Cells[0].Value);
                x.Value = S(r.Cells[1].Value);
                x.Secret = false;
                rows.Add(x);
            }
            shown.Rows = rows;
            Persist();
        }

        void Persist()
        {
            try { if (Vault.encKey != null) Vault.Save(boxes); }
            catch (Exception ex) { MessageBox.Show("Sauvegarde impossible : " + ex.Message, "Mon Coffre"); }
        }

        // ----- actions sur les cases -----
        void AddBox()
        {
            string[] v = Prompt.Show("Nouvelle case", "Donne un nom à ta nouvelle case.",
                new string[] { "Nom de la case" }, null, false);
            if (v == null || v[0].Trim().Length == 0) return;
            Box b = new Box();
            b.Title = v[0].Trim();
            b.Rows.Add(new Row { Label = "Identifiant", Value = "" });
            b.Rows.Add(new Row { Label = "Mot de passe", Value = "", Secret = true });
            b.Rows.Add(new Row { Label = "Note", Value = "" });
            boxes.Add(b);
            Persist();
            search.Text = "";
            RefreshList(b);
        }

        void RenameBox()
        {
            Box b = lst.SelectedItem as Box;
            if (b == null) return;
            string[] v = Prompt.Show("Renommer la case", "Nouveau nom de la case.",
                new string[] { "Nom de la case" }, new string[] { b.Title }, false);
            if (v == null || v[0].Trim().Length == 0) return;
            b.Title = v[0].Trim();
            Persist();
            RefreshList(b);
        }

        void DeleteBox()
        {
            Box b = lst.SelectedItem as Box;
            if (b == null) return;
            if (MessageBox.Show("Supprimer la case « " + b.Title + " » et tout son contenu ?\n(Elle ira dans la corbeille pendant 30 jours.)", "Mon Coffre",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            try { grid.EndEdit(); } catch (Exception) { }
            Sync();
            TrashItem gone = new TrashItem();
            gone.IsRow = false;
            gone.B = b;
            AddTrash(gone);
            shown = null;
            boxes.Remove(b);
            Persist();
            RefreshList(null);
        }

        void MoveBox(int d)
        {
            Box b = lst.SelectedItem as Box;
            if (b == null) return;
            if (search.Text.Trim().Length > 0)
            {
                MessageBox.Show("Vide la recherche pour réorganiser les cases.", "Mon Coffre");
                return;
            }
            int i = boxes.IndexOf(b), j = i + d;
            if (j < 0 || j >= boxes.Count) return;
            boxes[i] = boxes[j]; boxes[j] = b;
            Persist();
            RefreshList(b);
        }

        // ----- verrouillage rapide, voile, anti-capture -----
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.L))
            {
                LockNow();
                return true;
            }
            if (keyData == (Keys.Control | Keys.F))
            {
                search.Focus();
                search.SelectAll();
                return true;
            }
            if (keyData == Keys.Escape)
            {
                if ((DateTime.Now - lastEsc).TotalMilliseconds < 700)
                {
                    lastEsc = DateTime.MinValue;
                    LockNow();
                    return true;
                }
                lastEsc = DateTime.Now;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            veilTimer.Stop();
            veil.Visible = false;
        }

        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);
            if (Prefs.BlurOnLeave) veilTimer.Start();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            Prefs.ApplyCapture(this);
        }

        void ShowSecurityDialog()
        {
            using (Form f = new Form())
            {
                f.Text = "Sécurité";
                f.FormBorderStyle = FormBorderStyle.FixedDialog;
                f.StartPosition = FormStartPosition.CenterParent;
                f.MaximizeBox = false; f.MinimizeBox = false; f.ShowInTaskbar = false;
                f.Font = new Font("Segoe UI", 10f);
                f.ClientSize = new Size(500, 290);
                Label title = new Label();
                title.Text = "Options de sécurité";
                title.SetBounds(16, 14, 468, 24);
                CheckBox cbCap = new CheckBox();
                cbCap.Text = "Cacher la fenêtre des captures d'écran et du partage d'écran (elle apparaît noire ou invisible)";
                cbCap.Checked = Prefs.AntiCapture;
                cbCap.SetBounds(16, 48, 468, 48);
                CheckBox cbBlur = new CheckBox();
                cbBlur.Text = "Masquer le contenu dès que je clique sur une autre fenêtre ou application";
                cbBlur.Checked = Prefs.BlurOnLeave;
                cbBlur.SetBounds(16, 104, 468, 48);
                Label tip = new Label();
                tip.Text = "Verrouillage rapide : Ctrl+L, ou la touche Échap deux fois de suite.\nAstuce : pour m'envoyer une capture d'écran de l'application, désactive d'abord la première option.";
                tip.SetBounds(16, 164, 468, 60);
                Button ok = new Button();
                ok.Text = "Enregistrer";
                ok.DialogResult = DialogResult.OK;
                ok.SetBounds(286, 244, 110, 34);
                Button no = new Button();
                no.Text = "Annuler";
                no.DialogResult = DialogResult.Cancel;
                no.SetBounds(404, 244, 80, 34);
                f.Controls.Add(title); f.Controls.Add(cbCap); f.Controls.Add(cbBlur);
                f.Controls.Add(tip); f.Controls.Add(ok); f.Controls.Add(no);
                f.AcceptButton = ok; f.CancelButton = no;
                Theme.Apply(f);
                Theme.DarkTitle(f);
                ok.BackColor = Theme.AccentC;
                ok.FlatAppearance.BorderColor = Theme.AccentC;
                tip.ForeColor = Theme.MutedC;
                if (f.ShowDialog(this) != DialogResult.OK) return;
                Prefs.AntiCapture = cbCap.Checked;
                Prefs.BlurOnLeave = cbBlur.Checked;
                Prefs.Save();
                Prefs.ApplyCapture(this);
            }
        }

        // ----- fond d'ecran (image ou animation) -----
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (Backdrop.Mode == "aucun") { base.OnPaintBackground(e); return; }
            Backdrop.Draw(e.Graphics, e.ClipRectangle);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (Backdrop.Mode != "aucun" && ClientSize.Width > 1 && ClientSize.Height > 1)
            {
                Backdrop.Resize(ClientSize.Width, ClientSize.Height);
                Backdrop.Render();
                Invalidate(true);
            }
        }

        void ApplyBackdrop()
        {
            bool on = Backdrop.Mode != "aucun";
            foreach (Control p in frames) p.BackColor = on ? Color.Transparent : Theme.BgC;
            if (on)
            {
                Backdrop.Resize(ClientSize.Width, ClientSize.Height);
                Backdrop.Render();
            }
            if (Backdrop.Animated) animTimer.Start(); else animTimer.Stop();
            Invalidate(true);
        }

        // redessine seulement ce qui est derriere les boutons / le tableau / la liste (qui restent unis)
        void InvalidateBackdrop()
        {
            using (Region r = new Region(ClientRectangle))
            {
                ExcludeOpaque(this, r);
                Invalidate(r, true);
            }
        }

        void ExcludeOpaque(Control parent, Region r)
        {
            foreach (Control c in parent.Controls)
            {
                if (!c.Visible) continue;
                if (c is Button || c is TextBox || c is ListBox || c is DataGridView || c is StrengthBar)
                    r.Exclude(RectangleToClient(parent.RectangleToScreen(c.Bounds)));
                else
                    ExcludeOpaque(c, r);
            }
        }

        void ShowBackdropDialog()
        {
            string chosen = null;
            using (Form f = new Form())
            {
                f.Text = "Fond d'écran";
                f.FormBorderStyle = FormBorderStyle.FixedDialog;
                f.StartPosition = FormStartPosition.CenterParent;
                f.MaximizeBox = false; f.MinimizeBox = false; f.ShowInTaskbar = false;
                f.Font = new Font("Segoe UI", 10f);
                string[] labels = { "Aucun (couleur unie)", "🖼  Mon image…", "Bulles qui montent", "Matrix (pluie de caractères)", "➕  Autre… (11 fonds en plus — licence Pro)" };
                string[] modes = { "aucun", "image", "bulles", "matrix", "@autre" };
                int n = labels.Length;
                f.ClientSize = new Size(340, 56 + n * 46 + 60);
                Label l = new Label();
                l.Text = "Choisis ton fond :";
                l.SetBounds(16, 16, 308, 24);
                f.Controls.Add(l);
                for (int i = 0; i < n; i++)
                {
                    string m = modes[i];
                    Button b = new Button();
                    b.Text = labels[i] + (m == Backdrop.Mode ? "   ✔" : "");
                    b.SetBounds(16, 48 + i * 46, 308, 38);
                    b.Click += (s, e) => { chosen = m; f.Close(); };
                    f.Controls.Add(b);
                }
                Label note = new Label();
                note.Text = "Le fond se voit autour du tableau et de la liste, qui restent unis pour rester lisibles.";
                note.SetBounds(16, 48 + n * 46 + 6, 308, 44);
                f.Controls.Add(note);
                Theme.Apply(f);
                Theme.DarkTitle(f);
                note.ForeColor = Theme.MutedC;
                f.ShowDialog(this);
            }
            if (chosen == "@autre")
            {
                chosen = PickMoreBackdrop();
                if (chosen == null) return;
            }
            if (chosen == null) return;
            if (!Backdrop.IsFree(chosen) && !License.Activated) { ProOnlyMessage(); return; }
            if (chosen == "image")
            {
                using (OpenFileDialog d = new OpenFileDialog())
                {
                    d.Title = "Choisis ton image";
                    d.Filter = "Images (*.png;*.jpg;*.jpeg;*.bmp;*.gif)|*.png;*.jpg;*.jpeg;*.bmp;*.gif";
                    if (d.ShowDialog(this) != DialogResult.OK) return;
                    if (!Backdrop.SetImage(d.FileName))
                    {
                        MessageBox.Show("Impossible de lire cette image.", "Mon Coffre");
                        return;
                    }
                }
            }
            Backdrop.Mode = chosen;
            Backdrop.Save();
            ApplyBackdrop();
        }

        // ----- licence professionnelle -----
        void UpdateTitle()
        {
            Text = License.Activated ? License.Product + "  —  Licence professionnelle : " + License.Holder : "Mon Coffre";
        }

        void ActivateLicense()
        {
            string[] v = Prompt.Show("Activer ma licence",
                "Entre le nom du titulaire et la clé de licence reçue avec ton achat.\nFormat de la clé : MCPR-XXXXX-XXXXX-XXXXX-XXXXX",
                new string[] { "Nom du titulaire", "Clé de licence" }, new string[] { License.Holder, "" }, false);
            if (v == null) return;
            int res = License.Activate(v[0], v[1]);
            if (res == 0)
            {
                UpdateTitle();
                MessageBox.Show("Licence activée. Merci !\nTitulaire : " + License.Holder, "Mon Coffre");
            }
            else if (res == 2)
                MessageBox.Show("Licence déjà utilisée.\n\nCette clé a déjà été activée et ne peut pas l'être une deuxième fois. Contacte le vendeur pour obtenir une nouvelle clé.", "Mon Coffre", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (res == 3)
                MessageBox.Show("Impossible de contacter le serveur d'activation.\n\nVérifie ta connexion Internet puis réessaie. La clé n'a pas été consommée.", "Mon Coffre", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
                MessageBox.Show("Clé ou nom invalide. Vérifie que le nom est écrit exactement comme lors de l'achat.", "Mon Coffre");
        }

        void ShowLicenseDialog()
        {
            bool again = true;
            while (again)
            {
                again = false;
                bool doActivate = false, doDeactivate = false;
                using (Form f = new Form())
                {
                    f.Text = "Licence";
                    f.FormBorderStyle = FormBorderStyle.FixedDialog;
                    f.StartPosition = FormStartPosition.CenterParent;
                    f.MaximizeBox = false; f.MinimizeBox = false; f.ShowInTaskbar = false;
                    f.Font = new Font("Segoe UI", 10f);
                    f.ClientSize = new Size(620, 610);

                    Label title = new Label();
                    title.Text = License.Activated ? License.Product : "Mon Coffre";
                    title.Font = new Font("Segoe UI", 20f, FontStyle.Bold);
                    title.SetBounds(20, 12, 580, 40);
                    Label sub = new Label();
                    sub.Text = "Gestionnaire de mots de passe chiffré — édition professionnelle";
                    sub.SetBounds(20, 54, 580, 22);
                    Label status = new Label();
                    status.Font = new Font("Segoe UI", 11f, FontStyle.Bold);
                    status.SetBounds(20, 92, 580, 26);
                    Label info = new Label();
                    info.SetBounds(20, 122, 580, 44);
                    if (License.Activated)
                    {
                        status.Text = "✔  Licence professionnelle active";
                        info.Text = "Titulaire : " + License.Holder + "\nClé : " + License.MaskedKey();
                    }
                    else
                    {
                        status.Text = "Licence non activée — édition d'évaluation";
                        info.Text = "Entre ta clé de licence pour activer l'édition professionnelle.";
                    }
                    TextBox terms = new TextBox();
                    terms.Multiline = true;
                    terms.ReadOnly = true;
                    terms.ScrollBars = ScrollBars.Vertical;
                    terms.Text = License.Terms;
                    terms.SetBounds(20, 176, 580, 330);
                    Label foot = new Label();
                    foot.Text = "Version " + License.Version + "  —  © " + DateTime.Now.Year + " Mon Coffre. Tous droits réservés.";
                    foot.SetBounds(20, 514, 580, 22);

                    Button bAct = new Button();
                    bAct.Text = License.Activated ? "Changer de licence…" : "Activer une licence…";
                    bAct.SetBounds(20, 556, 200, 36);
                    Button bOff = new Button();
                    bOff.Text = "Désactiver";
                    bOff.SetBounds(230, 556, 140, 36);
                    bOff.Enabled = License.Activated;
                    Button bClose = new Button();
                    bClose.Text = "Fermer";
                    bClose.SetBounds(480, 556, 120, 36);
                    bClose.DialogResult = DialogResult.Cancel;
                    bAct.Click += (s, e) => { doActivate = true; f.Close(); };
                    bOff.Click += (s, e) => { doDeactivate = true; f.Close(); };

                    f.Controls.Add(title); f.Controls.Add(sub); f.Controls.Add(status); f.Controls.Add(info);
                    f.Controls.Add(terms); f.Controls.Add(foot);
                    f.Controls.Add(bAct); f.Controls.Add(bOff); f.Controls.Add(bClose);
                    f.CancelButton = bClose;
                    Theme.Apply(f);
                    Theme.DarkTitle(f);
                    status.ForeColor = License.Activated ? Theme.OkC : Color.FromArgb(249, 115, 22);
                    sub.ForeColor = Theme.MutedC;
                    foot.ForeColor = Theme.MutedC;
                    bAct.BackColor = Theme.AccentC;
                    bAct.FlatAppearance.BorderColor = Theme.AccentC;
                    f.ShowDialog(this);
                }
                if (doActivate)
                {
                    ActivateLicense();
                    again = true;
                }
                else if (doDeactivate)
                {
                    if (MessageBox.Show("ATTENTION : si tu désactives la licence, cette clé sera définitivement consommée.\n\nElle ne pourra plus jamais être réutilisée, et il faudra demander une nouvelle clé au vendeur.\n\nDésactiver quand même ?", "Mon Coffre",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        License.Deactivate();
                        UpdateTitle();
                        EnforceBackdrop();
                    }
                    again = true;
                }
            }
        }

        // ----- fonction Pro : audit de securite des mots de passe -----
        void ShowAudit()
        {
            if (!License.Activated) { ProOnlyMessage(); return; }
            try { grid.EndEdit(); } catch (Exception) { }
            Sync();
            List<string> wheres = new List<string>(), vals = new List<string>(), labs = new List<string>();
            foreach (Box b in boxes)
                foreach (Row r in b.Rows)
                {
                    if (!r.Secret || string.IsNullOrEmpty(r.Value)) continue;
                    string lab; Color col;
                    Strength.Evaluate(r.Value, out lab, out col);
                    wheres.Add(b.Title + "  ›  " + r.Label);
                    vals.Add(r.Value);
                    labs.Add(lab);
                }
            int total = wheres.Count;
            Dictionary<string, int> count = new Dictionary<string, int>();
            foreach (string v in vals) { int c; count.TryGetValue(v, out c); count[v] = c + 1; }
            StringBuilder weak = new StringBuilder(), reused = new StringBuilder();
            int bad = 0, nWeak = 0, nReused = 0;
            for (int i = 0; i < total; i++)
            {
                bool isWeak = labs[i] == "Très faible" || labs[i] == "Faible";
                bool isReused = count[vals[i]] > 1;
                if (isWeak) { nWeak++; weak.Append("   • " + wheres[i] + "   (" + labs[i] + ")\r\n"); }
                if (isReused) { nReused++; reused.Append("   • " + wheres[i] + "\r\n"); }
                if (isWeak || isReused) bad++;
            }
            int score = total == 0 ? 100 : (int)Math.Round(100.0 * (total - bad) / total);
            StringBuilder sb = new StringBuilder();
            sb.Append("SCORE DE SÉCURITÉ : " + score + " / 100\r\n");
            sb.Append(total + " mot(s) de passe analysé(s).\r\n\r\n");
            sb.Append("MOTS DE PASSE FAIBLES : " + nWeak + "\r\n");
            sb.Append(nWeak == 0 ? "   Aucun, bravo !\r\n" : weak.ToString());
            sb.Append("\r\nMOTS DE PASSE RÉUTILISÉS (le même sur plusieurs comptes) : " + nReused + "\r\n");
            sb.Append(nReused == 0 ? "   Aucun, bravo !\r\n" : reused.ToString());
            sb.Append("\r\nConseil : remplace chaque mot de passe de cette liste par un mot de passe unique, avec le bouton « Générer un mot de passe ».\r\n");
            sb.Append("L'analyse reste sur ton ordinateur : rien n'est envoyé nulle part.");
            using (Form f = new Form())
            {
                f.Text = "Audit des mots de passe";
                f.FormBorderStyle = FormBorderStyle.FixedDialog;
                f.StartPosition = FormStartPosition.CenterParent;
                f.MaximizeBox = false; f.MinimizeBox = false; f.ShowInTaskbar = false;
                f.Font = new Font("Segoe UI", 10f);
                f.ClientSize = new Size(620, 470);
                TextBox tb = new TextBox();
                tb.Multiline = true; tb.ReadOnly = true; tb.ScrollBars = ScrollBars.Vertical;
                tb.Text = sb.ToString();
                tb.SetBounds(16, 16, 588, 390);
                Button ok = new Button();
                ok.Text = "Fermer";
                ok.DialogResult = DialogResult.OK;
                ok.SetBounds(494, 418, 110, 34);
                f.Controls.Add(tb); f.Controls.Add(ok);
                f.AcceptButton = ok; f.CancelButton = ok;
                Theme.Apply(f);
                Theme.DarkTitle(f);
                f.ShowDialog(this);
            }
        }

        // ----- fonction Pro : sauvegardes automatiques a la fermeture (20 dernieres gardees) -----
        static string AutoDir { get { return System.IO.Path.Combine(Vault.DataDir, "Sauvegardes"); } }

        void AutoBackup()
        {
            if (!License.Activated) return;
            try
            {
                if (!File.Exists(Vault.DataFile)) return;
                Directory.CreateDirectory(AutoDir);
                string[] ex = Directory.GetFiles(AutoDir, "auto-*.coffre");
                Array.Sort(ex);
                if (ex.Length > 0 && (DateTime.Now - File.GetLastWriteTime(ex[ex.Length - 1])).TotalMinutes < 5) return;
                File.Copy(Vault.DataFile, System.IO.Path.Combine(AutoDir, "auto-" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".coffre"), true);
                ex = Directory.GetFiles(AutoDir, "auto-*.coffre");
                Array.Sort(ex);
                for (int i = 0; i < ex.Length - 20; i++) File.Delete(ex[i]);
            }
            catch (Exception) { }
        }

        void OpenAutoBackups()
        {
            if (!License.Activated) { ProOnlyMessage(); return; }
            try
            {
                Directory.CreateDirectory(AutoDir);
                MessageBox.Show("À chaque fermeture de Mon Coffre, une sauvegarde chiffrée est faite automatiquement (les 20 dernières sont gardées).\n\nPour en restaurer une : bouton « Importer », puis choisis un fichier de ce dossier. Elle s'ouvre avec ton mot de passe maître.", "Sauvegardes automatiques");
                System.Diagnostics.Process.Start("explorer.exe", "\"" + AutoDir + "\"");
            }
            catch (Exception) { }
        }

        void ProOnlyMessage()
        {
            MessageBox.Show("Vous n'avez pas accès, ceci est accessible avec la licence pro.", "Mon Coffre", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Si la licence n'est pas active, un fond Pro deja choisi est remplace par "aucun".
        void EnforceBackdrop()
        {
            if (License.Activated) return;
            if (!Theme.IsFree(Theme.Current))
            {
                Theme.SetStyle("Bleu");
                Theme.Save();
                ApplyTheme();
            }
            if (!Backdrop.IsFree(Backdrop.Mode))
            {
                Backdrop.Mode = "aucun";
                Backdrop.Save();
                ApplyBackdrop();
            }
        }

        // deuxieme panneau : les fonds en plus (bouton "Autre") - reserves a la licence Pro
        string PickMoreBackdrop()
        {
            string[] modes = { "aurore", "neige", "etoiles", "pluie", "vagues", "braises", "lucioles", "bokeh", "grille", "degrade", "ondes" };
            string[] labels = { "Aurore (dégradés qui bougent)", "Neige", "Étoiles", "Pluie", "Vagues", "Braises", "Lucioles", "Bokeh (lumières floues)", "Grille rétro", "Dégradé tournant", "Ondes" };
            string chosen = null;
            using (Form f = new Form())
            {
                f.Text = "Autres fonds";
                f.FormBorderStyle = FormBorderStyle.FixedDialog;
                f.StartPosition = FormStartPosition.CenterParent;
                f.MaximizeBox = false; f.MinimizeBox = false; f.ShowInTaskbar = false;
                f.Font = new Font("Segoe UI", 10f);
                int cols = 2, cw = 200, chh = 40, gap = 8;
                int rows = (modes.Length + cols - 1) / cols;
                f.ClientSize = new Size(32 + cols * cw + (cols - 1) * gap, 48 + rows * (chh + gap) + 12);
                Label l = new Label();
                l.Text = "Choisis un fond animé :";
                l.SetBounds(16, 16, 300, 24);
                f.Controls.Add(l);
                for (int i = 0; i < modes.Length; i++)
                {
                    string m = modes[i];
                    Button b = new Button();
                    b.Text = (License.Activated ? "" : "🔒 ") + labels[i] + (m == Backdrop.Mode ? "   ✔" : "");
                    b.SetBounds(16 + (i % cols) * (cw + gap), 48 + (i / cols) * (chh + gap), cw, chh);
                    b.Click += (s, e) =>
                    {
                        if (!License.Activated) { ProOnlyMessage(); return; }
                        chosen = m; f.Close();
                    };
                    f.Controls.Add(b);
                }
                Theme.Apply(f);
                Theme.DarkTitle(f);
                f.ShowDialog(this);
            }
            return chosen;
        }

        // ----- icone d'une case -----
        void ChooseIcon()
        {
            if (!License.Activated) { ProOnlyMessage(); return; }
            Box target = lst.SelectedItem as Box;
            if (target == null) return;
            string[] icons = {
                "🔑", "🔒", "🛡", "💳", "🏦", "💰", "🎮", "🕹",
                "📧", "💬", "📱", "💻", "🌐", "🛒", "🎬", "🎵",
                "🎧", "📚", "🎓", "💼", "🏠", "🚗", "✈", "🍕",
                "⚽", "📷", "☁", "⭐", "❤", "🔥", "💡", "📝",
                "📁", "🧾", "🎁", "🔧", "👤", "👥", "🐱", "🌈" };
            string chosen = null;
            using (Form f = new Form())
            {
                f.Text = "Icône de la case";
                f.FormBorderStyle = FormBorderStyle.FixedDialog;
                f.StartPosition = FormStartPosition.CenterParent;
                f.MaximizeBox = false; f.MinimizeBox = false; f.ShowInTaskbar = false;
                f.Font = new Font("Segoe UI", 10f);
                f.ClientSize = new Size(412, 48 + 5 * 44 + 56);
                Label l = new Label();
                l.Text = "Choisis une icône pour « " + target.Title + " » :";
                l.SetBounds(16, 14, 380, 24);
                f.Controls.Add(l);
                for (int i = 0; i < icons.Length; i++)
                {
                    string ic = icons[i];
                    Button b = new Button();
                    b.Font = new Font("Segoe UI Emoji", 14f);
                    b.Text = ic;
                    b.SetBounds(16 + (i % 8) * 48, 48 + (i / 8) * 44, 44, 40);
                    b.Click += (s, e) => { chosen = ic; f.Close(); };
                    f.Controls.Add(b);
                }
                Button none = new Button();
                none.Text = "Aucune icône";
                none.SetBounds(16, 48 + 5 * 44 + 8, 180, 34);
                none.Click += (s, e) => { chosen = ""; f.Close(); };
                f.Controls.Add(none);
                Theme.Apply(f);
                Theme.DarkTitle(f);
                f.ShowDialog(this);
            }
            if (chosen == null) return;
            target.Icon = chosen;
            Persist();
            lst.Invalidate();
        }

        // ----- force du mot de passe -----
        string CurrentValue()
        {
            DataGridViewCell c = grid.CurrentCell;
            if (c == null || c.RowIndex < 0 || c.RowIndex >= grid.Rows.Count) return "";
            return S(grid.Rows[c.RowIndex].Cells[1].Value);
        }

        void UpdateStrength(string v)
        {
            string label;
            Color col;
            int pct = Strength.Evaluate(v, out label, out col);
            strBar.Set(pct, col);
            lblStr.Text = label;
            lblStr.ForeColor = col;
        }

        void EditTextChanged(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb == null || grid.CurrentCell == null) return;
            if (grid.CurrentCell.ColumnIndex == 1) UpdateStrength(tb.Text);
        }

        // ----- corbeille -----
        void AddTrash(TrashItem t)
        {
            t.Ticks = DateTime.UtcNow.Ticks;
            Vault.Trash.Insert(0, t);
            while (Vault.Trash.Count > 100) Vault.Trash.RemoveAt(Vault.Trash.Count - 1);
        }

        void PurgeTrash()
        {
            long limit = DateTime.UtcNow.AddDays(-30).Ticks;
            int removed = Vault.Trash.RemoveAll(t => t.Ticks < limit);
            if (removed > 0) Persist();
        }

        static string DescribeTrash(TrashItem t)
        {
            string when = new DateTime(t.Ticks, DateTimeKind.Utc).ToLocalTime().ToString("dd/MM/yyyy HH:mm");
            if (t.IsRow)
                return "Ligne : " + (t.R.Label.Length > 0 ? t.R.Label : "(sans nom)") + "   (dans « " + t.ParentTitle + " »)   —   " + when;
            string ic = t.B.Icon.Length > 0 ? t.B.Icon + " " : "";
            int n = t.B.Rows.Count;
            return "Case : " + ic + t.B.Title + "   (" + n + (n > 1 ? " lignes" : " ligne") + ")   —   " + when;
        }

        void RestoreTrash(TrashItem t)
        {
            try { grid.EndEdit(); } catch (Exception) { }
            Sync();
            shown = null;
            Box target;
            if (t.IsRow)
            {
                target = null;
                foreach (Box b in boxes) if (b.Title == t.ParentTitle) { target = b; break; }
                if (target == null)
                {
                    target = new Box();
                    target.Title = t.ParentTitle.Length > 0 ? t.ParentTitle : "Restauré";
                    boxes.Add(target);
                }
                target.Rows.Add(t.R);
            }
            else
            {
                target = t.B;
                boxes.Add(target);
            }
            search.Text = "";
            Persist();
            RefreshList(target);
        }

        void ShowTrash()
        {
            List<TrashItem> view = new List<TrashItem>();
            using (Form f = new Form())
            {
                f.Text = "Corbeille";
                f.FormBorderStyle = FormBorderStyle.FixedDialog;
                f.StartPosition = FormStartPosition.CenterParent;
                f.MaximizeBox = false; f.MinimizeBox = false; f.ShowInTaskbar = false;
                f.Font = new Font("Segoe UI", 10f);
                f.ClientSize = new Size(600, 446);
                Label info = new Label();
                info.Text = "Les cases et lignes supprimées sont gardées 30 jours (chiffrées comme le reste).";
                info.SetBounds(16, 14, 568, 24);
                ListBox lb = new ListBox();
                lb.SetBounds(16, 46, 568, 300);
                lb.IntegralHeight = false;
                lb.HorizontalScrollbar = true;
                Button bRestore = new Button();
                bRestore.Text = "Restaurer";
                bRestore.SetBounds(16, 358, 130, 34);
                Button bDelete = new Button();
                bDelete.Text = "Supprimer définitivement";
                bDelete.SetBounds(156, 358, 210, 34);
                Button bEmpty = new Button();
                bEmpty.Text = "Vider la corbeille";
                bEmpty.SetBounds(376, 358, 208, 34);
                Button bClose = new Button();
                bClose.Text = "Fermer";
                bClose.SetBounds(454, 402, 130, 34);
                bClose.DialogResult = DialogResult.Cancel;
                f.Controls.Add(info); f.Controls.Add(lb);
                f.Controls.Add(bRestore); f.Controls.Add(bDelete); f.Controls.Add(bEmpty); f.Controls.Add(bClose);
                f.CancelButton = bClose;

                Action fill = () =>
                {
                    view.Clear();
                    lb.Items.Clear();
                    foreach (TrashItem it in Vault.Trash) view.Add(it);
                    foreach (TrashItem it in view) lb.Items.Add(DescribeTrash(it));
                    if (view.Count == 0) lb.Items.Add("(la corbeille est vide)");
                    bool any = view.Count > 0;
                    bRestore.Enabled = any; bDelete.Enabled = any; bEmpty.Enabled = any;
                };

                bRestore.Click += (s, e) =>
                {
                    int i = lb.SelectedIndex;
                    if (i < 0 || i >= view.Count) return;
                    TrashItem t = view[i];
                    RestoreTrash(t);
                    Vault.Trash.Remove(t);
                    Persist();
                    fill();
                };
                bDelete.Click += (s, e) =>
                {
                    int i = lb.SelectedIndex;
                    if (i < 0 || i >= view.Count) return;
                    if (MessageBox.Show("Supprimer définitivement cet élément ? Il ne pourra plus être récupéré.", "Mon Coffre",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
                    Vault.Trash.Remove(view[i]);
                    Persist();
                    fill();
                };
                bEmpty.Click += (s, e) =>
                {
                    if (MessageBox.Show("Vider toute la corbeille ? Tout sera perdu définitivement.", "Mon Coffre",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
                    Vault.Trash.Clear();
                    Persist();
                    fill();
                };

                fill();
                Theme.Apply(f);
                Theme.DarkTitle(f);
                f.ShowDialog(this);
            }
        }

        // ----- changer le style -----
        void ApplyTheme()
        {
            Theme.Apply(this);
            Theme.UpdateTitleBar(this);
            lblInfo.ForeColor = Theme.OkC;
            UpdateStrength(CurrentValue());
            ApplyBackdrop();
            grid.Invalidate();
            lst.Invalidate();
            Invalidate(true);
        }

        void ShowStyleDialog()
        {
            string chosen = null;
            bool custom = false;
            using (Form f = new Form())
            {
                f.Text = "Changer le style";
                f.FormBorderStyle = FormBorderStyle.FixedDialog;
                f.StartPosition = FormStartPosition.CenterParent;
                f.MaximizeBox = false; f.MinimizeBox = false; f.ShowInTaskbar = false;
                f.Font = new Font("Segoe UI", 10f);
                int n = Theme.Names.Length;
                int cols = 6, cw = 100, chh = 46, gap = 6;
                int rows = (n + cols - 1) / cols;
                f.ClientSize = new Size(32 + cols * cw + (cols - 1) * gap, 48 + rows * (chh + gap) + 62);
                Label l = new Label();
                l.Text = "Choisis une couleur :";
                l.SetBounds(16, 16, 400, 24);
                f.Controls.Add(l);
                Button[] bs = new Button[n];
                for (int i = 0; i < n; i++)
                {
                    string name = Theme.Names[i];
                    Button b = new Button();
                    b.Font = new Font("Segoe UI", 9f);
                    b.Text = (Theme.IsFree(name) || License.Activated ? "" : "🔒 ") + (name == Theme.Current ? name + " ✔" : name);
                    b.SetBounds(16 + (i % cols) * (cw + gap), 48 + (i / cols) * (chh + gap), cw, chh);
                    b.Click += (s, e) =>
                    {
                        if (!Theme.IsFree(name) && !License.Activated) { ProOnlyMessage(); return; }
                        chosen = name; f.Close();
                    };
                    f.Controls.Add(b);
                    bs[i] = b;
                }
                Button bc = new Button();
                bc.Text = (License.Activated ? "" : "🔒 ") + "🎨 Couleur libre…" + (Theme.Current == "Perso" ? " ✔" : "");
                bc.SetBounds(16, 48 + rows * (chh + gap) + 6, 230, 38);
                bc.Click += (s, e) =>
                {
                    if (!License.Activated) { ProOnlyMessage(); return; }
                    custom = true; f.Close();
                };
                f.Controls.Add(bc);
                Theme.Apply(f);
                Theme.DarkTitle(f);
                for (int k = 0; k < n; k++)
                {
                    string nm = Theme.Names[k];
                    Color c = Theme.PreviewColor(nm);
                    bool light = (c.R * 299 + c.G * 587 + c.B * 114) / 1000 > 150;
                    bs[k].BackColor = c;
                    bs[k].ForeColor = light ? Color.Black : Color.White;
                    bool dark = (c.R + c.G + c.B) < 150 || (c.R + c.G + c.B) > 700;
                    bs[k].FlatAppearance.BorderColor = nm == Theme.Current ? Color.White : (dark ? Color.FromArgb(110, 110, 110) : c);
                    bs[k].FlatAppearance.BorderSize = nm == Theme.Current ? 2 : 1;
                    bs[k].FlatAppearance.MouseOverBackColor = ControlPaint.Light(c, 0.3f);
                }
                f.ShowDialog(this);
            }
            if (custom)
            {
                using (ColorDialog cd = new ColorDialog())
                {
                    cd.FullOpen = true;
                    cd.Color = Theme.CustomColor;
                    if (cd.ShowDialog(this) != DialogResult.OK) return;
                    Theme.CustomColor = cd.Color;
                }
                chosen = "Perso";
            }
            if (chosen == null) return;
            Theme.SetStyle(chosen);
            Theme.Save();
            ApplyTheme();
        }

        // ----- creer / supprimer / copier une ligne -----
        void AddRow()
        {
            if (shown == null)
            {
                MessageBox.Show("Crée ou sélectionne d'abord une case.", "Mon Coffre");
                return;
            }
            try { grid.EndEdit(); } catch (Exception) { }
            Sync();
            Row nr = new Row();
            nr.Label = "Nouveau champ";
            shown.Rows.Add(nr);
            Persist();
            LoadGrid(shown);
            int last = grid.Rows.Count - 1;
            if (last >= 0)
            {
                grid.CurrentCell = grid.Rows[last].Cells[0];
                grid.BeginEdit(true);
            }
        }

        void DeleteRow()
        {
            if (shown == null) return;
            DataGridViewCell c = grid.CurrentCell;
            if (c == null)
            {
                MessageBox.Show("Sélectionne d'abord une ligne.", "Mon Coffre");
                return;
            }
            try { grid.EndEdit(); } catch (Exception) { }
            Sync();
            int i = c.RowIndex;
            if (i < 0 || i >= shown.Rows.Count) return;
            if (MessageBox.Show("Supprimer la ligne « " + shown.Rows[i].Label + " » ?\n(Elle ira dans la corbeille pendant 30 jours.)", "Mon Coffre",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            Row goneRow = shown.Rows[i];
            if (goneRow.Label.Length > 0 || goneRow.Value.Length > 0)
            {
                TrashItem ti = new TrashItem();
                ti.IsRow = true;
                ti.ParentTitle = shown.Title;
                ti.R = goneRow;
                AddTrash(ti);
            }
            shown.Rows.RemoveAt(i);
            Persist();
            LoadGrid(shown);
        }

        void CopyRow(int idx)
        {
            if (idx < 0 || idx >= grid.Rows.Count) return;
            try { grid.EndEdit(); } catch (Exception) { }
            DataGridViewRow row = grid.Rows[idx];
            if (row.IsNewRow) return;
            // on copie la valeur ; si elle est vide, on copie le nom de la ligne
            string v = S(row.Cells[1].Value);
            if (v.Trim().Length == 0) v = S(row.Cells[0].Value);
            if (v.Trim().Length == 0) { ShowInfo("Rien à copier sur cette ligne"); return; }
            SetClipboard(v);
        }

        void ShowInfo(string msg)
        {
            lblInfo.Text = msg;
            infoTimer.Stop();
            infoTimer.Start();
        }

        // ----- deplacer une ligne -----
        void MoveRow(int d)
        {
            if (shown == null) return;
            DataGridViewCell c = grid.CurrentCell;
            if (c == null || c.OwningRow.IsNewRow)
            {
                MessageBox.Show("Sélectionne d'abord une ligne.", "Mon Coffre");
                return;
            }
            try { grid.EndEdit(); } catch (Exception) { }
            Sync();
            int i = c.RowIndex, j = i + d;
            if (j < 0 || j >= shown.Rows.Count || i >= shown.Rows.Count) return;
            Row tmp = shown.Rows[i];
            shown.Rows[i] = shown.Rows[j];
            shown.Rows[j] = tmp;
            Persist();
            LoadGrid(shown);
            grid.CurrentCell = grid.Rows[j].Cells[0];
        }

        // ----- presse-papier -----
        void SetClipboard(string v)
        {
            try
            {
                Clipboard.SetDataObject(v, true, 5, 100);
                lastCopied = v;
                ShowInfo("Copié ✔ (effacé du presse-papiers dans 20 s)");
                clipTimer.Stop(); clipTimer.Start();
            }
            catch (Exception) { MessageBox.Show("Copie impossible.", "Mon Coffre"); }
        }

        void ClearClip()
        {
            clipTimer.Stop();
            try
            {
                if (lastCopied != null && Clipboard.ContainsText() && Clipboard.GetText() == lastCopied) Clipboard.Clear();
            }
            catch (Exception) { }
            lastCopied = null;
            lblInfo.Text = "";
        }

        void CopyValue()
        {
            DataGridViewCell c = grid.CurrentCell;
            if (c == null || c.OwningRow.IsNewRow)
            {
                MessageBox.Show("Sélectionne d'abord une ligne.", "Mon Coffre");
                return;
            }
            string v = S(grid.Rows[c.RowIndex].Cells[1].Value);
            if (v.Length == 0) return;
            SetClipboard(v);
        }

        static string GenPw(int n)
        {
            const string set = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnpqrstuvwxyz23456789!@#$%&*?-_";
            StringBuilder sb = new StringBuilder();
            int lim = 256 - (256 % set.Length);
            byte[] b = new byte[1];
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                while (sb.Length < n)
                {
                    rng.GetBytes(b);
                    if (b[0] < lim) sb.Append(set[b[0] % set.Length]);
                }
            }
            return sb.ToString();
        }

        void Generate()
        {
            string pw = GenPw(20);
            DialogResult r = MessageBox.Show(
                "Mot de passe généré :\n\n" + pw + "\n\nOui = l'écrire dans la ligne sélectionnée\nNon = le copier seulement\nAnnuler = fermer",
                "Générateur", MessageBoxButtons.YesNoCancel);
            if (r == DialogResult.Cancel) return;
            if (r == DialogResult.Yes)
            {
                DataGridViewCell c = grid.CurrentCell;
                if (c != null && !c.OwningRow.IsNewRow)
                {
                    try { grid.EndEdit(); } catch (Exception) { }
                    grid.Rows[c.RowIndex].Cells[1].Value = pw;
                }
                else MessageBox.Show("Sélectionne d'abord une ligne existante.", "Mon Coffre");
            }
            SetClipboard(pw);
        }

        // ----- mot de passe maitre / sauvegarde -----
        void ChangeMaster()
        {
            while (true)
            {
                string[] v = Prompt.Show("Mot de passe maître", "Choisis le nouveau mot de passe maître (8 caractères minimum).",
                    new string[] { "Nouveau mot de passe", "Confirmation" }, null, true);
                if (v == null) return;
                if (v[0].Length < 8) { MessageBox.Show("8 caractères minimum.", "Mon Coffre"); continue; }
                if (v[0] != v[1]) { MessageBox.Show("Les deux mots de passe ne correspondent pas.", "Mon Coffre"); continue; }
                try { grid.EndEdit(); } catch (Exception) { }
                Sync();
                Vault.NewKeys(v[0]);
                Persist();
                MessageBox.Show("Mot de passe maître changé.", "Mon Coffre");
                return;
            }
        }

        void ExportBackup()
        {
            try { grid.EndEdit(); } catch (Exception) { }
            Sync();
            using (SaveFileDialog d = new SaveFileDialog())
            {
                d.Filter = "Sauvegarde Mon Coffre (*.coffre)|*.coffre";
                d.FileName = "sauvegarde-" + DateTime.Now.ToString("yyyy-MM-dd") + ".coffre";
                if (d.ShowDialog(this) != DialogResult.OK) return;
                try
                {
                    File.Copy(Vault.DataFile, d.FileName, true);
                    MessageBox.Show("Sauvegarde enregistrée (elle est chiffrée avec ton mot de passe maître).", "Mon Coffre");
                }
                catch (Exception ex) { MessageBox.Show("Échec : " + ex.Message, "Mon Coffre"); }
            }
        }

        void ImportBackup()
        {
            using (OpenFileDialog d = new OpenFileDialog())
            {
                d.Filter = "Sauvegarde Mon Coffre (*.coffre)|*.coffre|Tous les fichiers (*.*)|*.*";
                if (d.ShowDialog(this) != DialogResult.OK) return;
                if (MessageBox.Show("Cette sauvegarde va REMPLACER tout le contenu actuel du coffre. Continuer ?", "Mon Coffre",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
                string[] v = Prompt.Show("Importer", "Entre le mot de passe maître utilisé pour cette sauvegarde.",
                    new string[] { "Mot de passe" }, null, true);
                if (v == null) return;
                try
                {
                    byte[] data = File.ReadAllBytes(d.FileName);
                    List<Box> nb = Vault.Open(data, v[0]);
                    shown = null;
                    boxes = nb;
                    Persist();
                    RefreshList(null);
                    MessageBox.Show("Sauvegarde importée. Ton mot de passe maître est maintenant celui de cette sauvegarde.", "Mon Coffre");
                }
                catch (Exception) { MessageBox.Show("Fichier invalide ou mot de passe incorrect.", "Mon Coffre"); }
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            try { grid.EndEdit(); Sync(); } catch (Exception) { }
            ClearClip();
            AutoBackup();
            base.OnFormClosing(e);
        }
    }

    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Theme.Load();
            Backdrop.Load();
            Prefs.Load();
            License.Load();
            if (!License.Activated && !Theme.IsFree(Theme.Current)) { Theme.SetStyle("Bleu"); Theme.Save(); }
            if (!License.Activated && !Backdrop.IsFree(Backdrop.Mode)) { Backdrop.Mode = "aucun"; Backdrop.Save(); }
            MainForm f = new MainForm();
            if (!f.Unlock()) return;
            Application.Run(f);
        }
    }
}
