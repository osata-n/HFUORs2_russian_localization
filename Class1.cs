using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine;
using System;
using System.Threading;

[BepInPlugin("MyTranslator", "Translation Mod", "1.0.0")]
public class TranslationPlugin : BaseUnityPlugin
{
    public static ManualLogSource logger;
    private static Dictionary<string, string> translations = new Dictionary<string, string>();
    private static HashSet<string> loggedTexts = new HashSet<string>();
    
    void Awake()
    {
        logger = Logger;
        Logger.LogInfo("Translation mod loaded!");

        string pluginPath = Path.GetDirectoryName(Info.Location);
        string translationsDir = Path.Combine(pluginPath, "translations");
        string untranslatedFile = Path.Combine(translationsDir, "untranslated.txt");
        
        try
        {
            if (File.Exists(untranslatedFile))
            {
                File.Delete(untranslatedFile);
                Logger.LogInfo("Файл untranslated.txt очищен при запуске.");
            }
        }
        catch (Exception ex)
        {
            Logger.LogWarning($"Не удалось очистить untranslated.txt: {ex.Message}");
        }

        LoadTranslations();
        Harmony.CreateAndPatchAll(typeof(TextTranslationPatch));
        Harmony.CreateAndPatchAll(typeof(SceneChangePatcher));
    }
    
    void LoadTranslations()
    {
        translations.Clear();

        TextTranslationPatch.ResetTranslationCache();

        System.Threading.Thread.Sleep(50);
        
        AddDefaultTranslations();
        
        string pluginPath = Path.GetDirectoryName(Info.Location);
        string translationsPath = Path.Combine(pluginPath, "translations");
        
        Logger.LogInfo($"Looking for translations in: {translationsPath}");
        
        if (!Directory.Exists(translationsPath))
        {
            Logger.LogWarning("Translations directory not found! Creating default...");
            CreateDefaultTranslations(translationsPath);
            return;
        }
        
        string[] translationFiles = Directory.GetFiles(translationsPath, "*.txt");
        
        foreach (string file in translationFiles)
        {
            LoadTranslationFile(file);
        }
        
        Logger.LogInfo($"Loaded {translations.Count} translations from {translationFiles.Length} files");
        CreateUntranslatedFile(translationsPath);
    }
    
    void AddDefaultTranslations()
    {
        translations["Are you okay?"] = "С тобой все в порядке?";
        translations["Touch the circle to the border!"] = "Коснись круга внтури границы!";
        translations["Touch the circle!"] = "Коснитесь круга!";
        translations["Im sorry..."] = "Простите...";
        translations["Right?!"] = "Точно?!";
        translations["Happy now?"] = "Довольны теперь?";
        translations["Why?"] = "Почему?";
        translations["Hold on..."] = "Постой...";
        translations["Arm"] = "Рука";
        translations["Head"] = "Голова";
        translations["Knee"] = "Колени";
        translations["Ugh, seriously..."] = "Ой, серьезно...";
        translations["Green"] = "Зеленый";
        translations["Huh? Me?"] = "А? Со мной?";
        translations["White"] = "Белый";
        translations["Black"] = "Черный";
        translations["Blue"] = "Синий";
        translations["Yellow"] = "Желтый";
        translations["Pink"] = "Розовый";
        translations["Red"] = "Красный";
        translations["Got it."] = "Поняла.";
        translations["Got it!"] = "Попалась!";
        translations["You sure about that?"] = "Уверен?";
        translations["Here you go."] = "Держите.";
        translations["Exactly… but after talking, "] = "Именно… но после разговора,";
        translations["— Just explain the whole situation clearly and ask them to help set the record straight…\n—  Eek!!"] = "Просто объясни ей всю ситуацию четко и попроси помочь все прояснить…\n— Ай!!";
        translations["Um, sorry, but… "] = "Эм, извините, но…";
        translations["but I couldn’t think of anything, "] = "но не могла придумать, что именно,";
        translations["I couldn’t think of anything, "] = "Я не могла придумать что именно,";
        translations["You don’t seem to be eating much lately… "] = "Вы, кажется, в последнее время мало едите…";
        translations["took care of you. "] = "ухаживала за вами.";
        translations["Hello... "] = "Здравствуйте...";
        translations["I even remember "] = "Я даже помню";
        translations["Look! "] = "Смотри!";
        translations["Wait, then... "] = "Стой, тогда...";
        translations["Tell her "] = "Сказать ей";
        translations["Push her "] = "Оттолкнуть ее";
        translations["Don’t worry. "] = "Не волнуйся.";
        translations["She was already super slim, "] = "Она и так была очень стройной,";
        translations["Right?! "] = "Верно?!";
        translations["I’m spinning... "] = "Все кружится...";
        translations["I just... would rather "] = "Я просто... предпочел бы";
        translations["Really... "] = "По правде говоря...";
        translations["Back when we were kids, "] = "Когда мы были детьми,";
        translations["What? No, come on— "] = "Что? Нет, да ладно —";
        translations["No, no look, I ended up with it somehow, okay? "] = "Нет, нет, послушай, она у меня как-то сама оказалась, ясно?";
        translations["Well… I’ve seen Yu-jeong working at "] = "Ну… я видел, как Ючжон работает";
        translations["You know, I think you look "] = "Знаешь, я думаю, ты выглядишь";
        translations["That’s too far. "] = "Это уже перебор.";
        translations["And since I am a magician... "] = "А раз уж я фокусник...";
        translations["Go to Mal-sook "] = "Подойти к Маль-сук";
        translations["Yeah, like… "] = "Да, типа…";
        translations["Alright, let’s see "] = "Хорошо, давайте посмотрим";
        translations["No, no, it’s fine… "] = "Нет, нет, все в порядке…";
        translations["Do you really think this mess is acceptable? "] = "Вы действительно думаете, что такой бардак приемлем?";
        translations["Mal-sook's Room"] = "Комната\nМаль-сук";
        translations["Milk Guys Magazine"] = "Журнал\n“Milk Guys”";
        translations["Give soondae (blood sausage)"] = "Дать сундэ\n(кровяную колбасу)";
        translations["Go to Min-jung’s Room"] = "Идти в комнату\nМин-Чжун";
        translations["Run the Errand"] = "Выполнить\nпоручение";
        translations["Heartflutter\nMoment"] = "Вспышка\nчувств";
        translations["Affection\nSearch"] = "Прогресс\nглавы";
        translations["(Boarding House Flier)\nJust finishing this up before heading home."] = "(Листовка пансиона)\nЗаканчиваю и сразу домой.";
        translations["—Open your eyes! Come on, keep them open!\n—Ahh... ah..."] = "—Открой глаза! Давай, не закрывай!\n—Ах... а...";
        translations["—Keep your eyes open! Stay with us!\n—Ahh... it... it hurts..."] = "—Не закрывай глаза! Будь с нами!\n—Ай... бо... больно...";
        translations["(Boarding House Flier)\nSo it was the flyer run that got me into the accident..."] = "(Листовка пансиона)\nТак это из-за раздачи листовок я попал в аварию...";
        translations["Main Info\nSearch"] = "Поиск\nинформации";
        translations["- AAAAK! Argh!!!\n- Sir, are you okay??"] = "- ААААЙ! Аргх!!!\nМолодой человек, с вами все в порядке??";
        translations["A pair of panties left on the hospital couch...\nWhose are these?"] = "Брошенные на больничном диване трусики...\nЧьи они?";
        translations["A perfectly ripe banana.\nWhat on earth am I supposed to do with this?"] = "Идеально спелый банан.\nИ что мне, спрашивается, с ним делать?";
        translations["(The Art of Fighting)\nHm?"] = "(Искусство боя)\nХм?";
        translations["(The Art of Fighting)\nWhy’s this here?"] = "(Искусство боя)\nЧто оно здесь делает?";
        translations["These must be the nurse Bang's shoes.\nThey're so small—what size is she?"] = "Должно быть, это туфли медсестры Пан.\nТакие маленькие — какой у нее размер?";
        translations["— Kids~\n— Ah, you scared me!"] = "— Дети~\nАй, ты напугала меня!?";
        translations["— See? Look! Look!\n— You wanna see? Wanna see!?"] = "— Видишь? Смотри! Смотри!\n— Хочешь увидеть? Хочешь увидеть!?";
        translations["- What the hell are you saying?\n- <O warrior, redder than fire itself>"] = "— Что ты, черт возьми, несешь?\n<О воин, краснее самого пламени>";
        translations["- What? You’re sick!\n- You can’t go by yourself!"] = "– Что? Ты же еще нездоров!\n– Нельзя тебе одному ходить!";
        translations["(Min-jung's Concert Invitation)\nWhoa... whoa! Yes!! I have to go to this...!"] = "(Приглашение на концерт Мин-Чжун)\nОго... ого! Да!! Я обязательно должен туда пойти...!";
        translations["–Wait...\n–Hold on..."] = "– Стой...\n– Подожди...";
        translations["- Aagh—!\n- Ah—!"] = "– Айй—!\n– Ах—!";
        translations["–Hello!\n–Hi..."] = "–Здравствуйте!\n–Добрый день!";
        translations["Smells like rich cocoa...\nWho should I gift this to?"] = "Пахнет дорогим какао...\nКому бы это подарить?";
        translations["When someone asked\nif we were newlyweds... and I said “Yes.”"] = "Когда кто-то спросил,\nпоженились ли мы недавно... и ты ответил “Да”";
        translations["When I nailed the presentation\nand you patted my head."] = "Когда я отлично выступила с презентацией,\nи ты погладил меня по голове.";
        translations["— Huh? That’s Park Min-jung!\n— Crap, they saw us..."] = "— Хэ? Это же Пак Мин-Чжун!\n— Черт, они нас заметили...";
        translations["— Huh? That’s Park Min-jung!\n— Hurry, go!!"] = "— Хэ? Это же Пак Мин-Чжун!\n— Быстрее, бежим!!";
        translations["— Well, anywa—wait, what?\n— Min-jung, should we get going?"] = "— Ну, в любом слу—стой, что?\n— Мин-Чжун, нам, может, уже пора?";
        translations["Her name’s Yu-jeong...\nI really should give this back."] = "Ее зовут Ючжон...\nНадо бы это вернуть.";
        translations["This world’s a mess...\nGuess I’d better learn how to protect someone."] = "Мир в полном бардаке...\nПожалуй, стоит научиться кого-то защищать.";
        translations["— Man, the weather’s so nice today.\n— Huh? Isn’t that Mal-sook...?"] = "— Чувак, какая сегодня хорошая погода.\n— А? Это разве не Маль-сук...?";
        translations["— Pfft!\n— What?"] = "— Пффф!\n— Что?";
        translations["— She just hung up on me?!\n— A friend."] = "— Она бросила трубку?!\n— Друг.";
        translations["— What is even happening?\n— What are they doing??"] = "— Что вообще происходит?\n— Что они делают??";
        translations["— Oh!\n— Ah!"] = "— Ой!\n— Ай!";
        translations["— Min-jung, come on, you can’t start crying now!\n— Oh come on, seriously?!"] = "— Мин-Чжун, ну же, нельзя же начинать плакать сейчас!\n— Ой, да ну, серьезно?!";
        translations["— You call yourself a man?\n— Min-jung!!"] = "— И ты называешь себя мужчиной?\n— Мин-Чжун!!";
        translations["— Huhhh~?\n— Hmm..."] = "— Нууууу~?\n— Хм...";
        translations["— Huh? Wait, what...?\n— Ah..."] = "— А? Погоди, что...?\n— Ах...";
        translations["— Gasp! No way!\n— Wow!"] = "— Ой! Не может быть!\n— Вау!";
        translations["— Haa...\n— Phew..."] = "— Хаа...\n— Фух...";
        translations["— Gasp!\n— Huh...?"] = "— Ах!\n— Что...?";
        translations["— House cleaning: furniture, tops of wardrobes, floors…\n— Just call the service already."] = "— Уборка дома: мебель, верх шкафов, полы…\n— Да просто уже вызовите сервис.";
        translations["— Kitchen vents, living room, master bedroom, bathroom, garden…\n— How much would that cost?"] = "— Кухонные вытяжки, гостиная, спальня, ванная, сад…\n— Сколько это будет стоить?";
        translations["— Kitchen vents, living room, master bedroom, bathroom, garden…\n— How much would that cost? I’ll pay for it; it’s just to make things easier."] = "— Кухонные вытяжки, гостиная, спальня, ванная, сад…\n— Сколько это будет стоить? Я заплачу; просто чтобы было проще.";
        translations["— Kitchen vents, living room, master bedroom, bathroom, garden…\n— Okay, let's do that then."] = "— Кухонные вытяжки, гостиная, спальня, ванная, сад…\n— Хорошо, тогда давайте так и сделаем.";
        translations["— Hahaha!!!\n— Hahaha… hahahahaha!!!"] = "— Ха-ха-ха!!!\n— Ха-ха-ха… ха-ха-ха-ха-ха!!!";
        translations["(Milk Corporation Report)\nReport on Product Manufacturing Process Innovation?"] = "(Отчет “Milk Corporation”)\nОтчет об инновации производственного процесса продукции?";
        translations["(MilkFilm Channel)\nMilk Films?"] = "(Канал “MilkFilm”)\n“Milk Films”?";
        translations["Search"] = "Поиск";
        translations["Help"] = "Помощь";
        translations["Huh? Why?"] = "А? А что?";
        translations["Ugh, I just had to deal with that guy this month, "] = "Уф, мне только в этом месяце пришлось иметь дело с этим парнем";
        translations["— Haa…\n— What now…"] = "— Ха…\n— И что теперь…";
        translations["— Ugh… what do we even do...\n— Guys~"] = "— Уф… что вы творите...\n— Ребята~";
        translations["— Guys\n— That’s enough...!"] = "— Ребята\n— Хватит уже...!";
        translations["Feels like your center of gravity’s off or something. Gets hit by Seung-yeon again"] = "Чувствуется, будто центр тяжести у тебя сбит.\n*Снова получает удар от Сын-Ена*";
        translations["God, I nearly died trying not to laugh. laughs"] = "Боже, я чуть не умерла, сдерживая смех.\n*смеется*";
        translations["At home, no one even treats me like a person~ *cries*"] = "Дома меня даже за человека не считают~\n*рыдает*";
        translations["Don’t badmouth my family *cries*"] = "Не говорите плохо о моей семье\n*рыдает*";
        translations["I’m sorry *cries*"] = "Прости\n*рыдает*";
        translations["I… I even abandoned my job, abandoned my fans *cries*"] = "Я… я вообще бросила работу, бросила своих фанатов\n*рыдает*";
        translations["— Pitiful…\n— Pitiful…"] = "— Бедные мы…\n— Бедные мы…";
        translations["Yeah. We’ve got work tomorrow, "] = "Да. У нас ведь завтра работа,";
        translations["— If it’s a towel, I’ve got one!\n— Ah! Found it!"] = "— Если нужно полотенце, у меня есть!\n— А! Нашла!";
        translations["Manager, just doing this properly "] = "Менеджер, если делать это правильно,";
        translations["Ah~"] = "Ах~";
        translations["When it comes to fruit, kiwi’s the best—say "] = "Когда дело доходит до фруктов, киви — лучший, скажи же";
        translations["— AAAAAAHHHH!!!\n— WAAAAAAHHHHH!!!"] = "— АААААААЙЙЙ!!!\n— АААААААААХ!!!";
        translations["Talk"] = "Сказать";
        translations["— What was that!?\n— Huh…!?"] = "— Что это было!?\n— Что…!?";
        translations["What is it?"] = "Что это?";
        translations["WAAAAHHHH!!!\nWait! Don’t leave me!!"] = "—ААААХ!!!\nСтой! Не бросай меня!!";
        translations["- AAAAAAAAHHH!!!!\n- WAAAAAAAHHH!!!!"] = "— ААААААААЙЙЙ!!!!\n— ААААААААХ!!!!";
        translations["You sure?"] = "Уверен?";
        translations["Hey, even the teacher left his gold toad statue at home this morning.\nJust let me off this once, please..."] = "ты правда думаешь, что все можно.\n-Простите один раз, пожалуйста…";
        translations["-Hey, even the teacher left his gold toad statue at home this morning.\n-Just this once...! Ah!"] = "-И не подумаю.\nНу всего один раз...! А!";
        translations[" and I kinda got lost on the way here..."] = "и я заблудилась по дороге сюда…";
        translations["Ah! Really? Hey, go quickly. "] = "А! Правда? Эй, беги быстрее.";
        translations["The discipline teacher’s still out front, "] = "Дежурный учитель еще у главного входа,";
        translations["Ah! Sorry!"] = "Ай! Простите!";
        translations["—Thank you.\n—Wait... what?"] = "—Спасибо.\n—Погодите... что?";
        translations["—Uh...!\n—Whoa, hey?!"] = "—Э-э...!\n—Воу, эй?!";
        translations["White?"] = "Белые?";
        translations["Wait, then... "] = "Стой, тогда...";
        translations["Make a mega skewer"] = "Сделать мега-шашлык";
        translations["It’s fine, really. "] = "Все в порядке, честно.";
        translations["- Whoa!! \n- Whoa!"] = "- Вау!!\n- Ого!";
        translations["Sounds good."] = "Без проблем.";
        translations["Take a look."] = "Дай гляну.";
        translations["- Hurry up! \n- Yeah, hurry up!"] = "- Давай же!\n- Да, давай!";
        translations["- Come on! Hurry up! \n- Hurry up already!"] = "- Давай же! Давай!\n- Да, быстрее уже!";
        translations["- Cheers!\n- Ugh..."] = "- За нас!\n- Угх...";
        translations["- Ugh, what was that... \n- Isn’t that how you do it?"] = "- Фух, что это было...\n- Разве так делается?";
        translations["- Hurry up! Hurry up! \n- Yeah, come on already!"] = "- Давай! Давай!\n- Да, ну же!";
        translations["- We are! The keepers of each other's dreams, hopes, futures, and passions \n- Okay, okay, let’s just cheers!"] = "- Мы — хранители мечт, надежд, будущего и страстей друг друга!\n- Ладно, ладно, давайте просто выпьем!";
        translations["- We are! The keepers of each other's dreams, hopes, futures, and passions\n- Just drink already!"] = "- Мы — хранители мечт, надежд, будущего и страстей друг друга!\n- Да, пейте уже!";
        translations["- We lift each other up, root for each other, and sponsor one another— \n-  I already took the shot."] = "- Мы поднимаем друг друга, болеем друг за друга и поддерживаем один другого—\n- Я уже выпила.";
        translations["- We pitch in, back each other up, and worry when needed\n- Nice, nice! Well done!"] = "- Мы вкладываемся, прикрываем друг другу спину и беспокоимся, когда нужно\n- Класс, класс! Молодец!";
        translations["- We stress for each other, worry together, and wish the best for one another\n- Come on, eat already."] = "- Мы переживаем друг за друга, волнуемся вместе и желаем друг другу всего наилучшего\n- Давайте, ешьте уже.";
        translations["- We stress for each other, worry together, and wish the best for one another \n- Did everyone take their shot?"] = "- Мы переживаем друг за друга, волнуемся вместе и желаем друг другу всего наилучшего\n- Все уже выпили?";
        translations["- We stress for each other, worry together, and wish the best for one another \n- That looks tasty."] = "- Мы переживаем друг за друга, волнуемся вместе и желаем друг другу всего наилучшего\n- Выглядит вкусно.";
        translations["- We stress for each other, worry together, and wish the best for one another\n- Gran didn’t drink yet!"] = "- Мы переживаем друг за друга, волнуемся вместе и желаем друг другу всего наилучшего\n- Гран еще не выпила!";
        translations["- Hurry up! Hurry up! \n- Yeah, do it already!"] = "- Давай! Давай!\n- Да, начинай уже!";
        translations["- Cheers!\n- Cheers!"] = "- За нас!\n- За нас!";
        translations["- Ah~ \n- Ah~"] = "“- А~”\n“- А~”";
        translations["- Ta-da~ \n- Whoa! You scared me!"] = "- Та-да~\nУх! Вы меня напугали!";
        translations["- Happy birthday! \n- Happy birthday!"] = "- С днем рождения!\n- С днем рождения!";
        translations["- Happy birthday dear Yuman~\n- Happy birthday to you~"] = "- С днем рождения, дорогой Юман~\n- С днем рождения тебя~";
        translations["Coming!"] = "Иду!";
        translations[" ....tracking your farts all day? Huh?!"] = "...отслеживать твои пуки целый день? А?!";
        translations["Hello..."] = "Алло...";
    }
    
    void LoadTranslationFile(string filePath)
    {
        try
        {
            int loadedCount = 0;
            int attempts = 0;
            string[] lines = null;

            while (attempts < 3 && lines == null)
            {
                try
                {
                    lines = File.ReadAllLines(filePath);
                }
                catch (IOException)
                {
                    attempts++;
                    if (attempts < 3)
                    {
                        Logger.LogInfo($"Retrying to read {filePath} (attempt {attempts})");
                        Thread.Sleep(100);
                    }
                }
            }
            
            if (lines == null)
            {
                Logger.LogError($"Failed to read translation file: {filePath}");
                return;
            }

            var keysToRemove = new List<string>();
            foreach (var kvp in translations)
            {
                foreach (string line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line) || 
                        line.StartsWith("//") || 
                        line.StartsWith("#"))
                        continue;
                        
                    string trimmedLine = line.Trim();
                    if (trimmedLine.StartsWith(kvp.Key + "="))
                    {
                        keysToRemove.Add(kvp.Key);
                        break;
                    }
                }
            }
            
            foreach (var key in keysToRemove)
            {
                translations.Remove(key);
            }
            
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line) || 
                    line.StartsWith("//") || 
                    line.StartsWith("#"))
                    continue;
                    
                string[] parts = line.Split(new char[] { '=' }, 2);
                
                if (parts.Length == 2)
                {
                    string key = parts[0].Trim();
                    string value = parts[1].Trim();
                    
                    translations[key] = value;
                    loadedCount++;
                    
                    Logger.LogDebug($"Loaded: '{key}' = '{value}'");
                }
            }
            
            Logger.LogInfo($"Loaded/Updated {loadedCount} translations from: {Path.GetFileName(filePath)}");
        }
        catch (System.Exception e)
        {
            Logger.LogError($"Error loading translation file {filePath}: {e.Message}\n{e.StackTrace}");
        }
    }
    
    void CreateDefaultTranslations(string translationsPath)
    {
        Directory.CreateDirectory(translationsPath);
        
        string[] uiTranslations = {
            "// UI Translations for Five Hearts Under One Roof season 2",
            "// Format: EnglishText=RussianTranslation",
            "// Add your translations below:",
            "// Note: 'Are you okay?' already has default translation in code",
            "Touch the circle to the border!=Коснись круга внутри границы!",
            "Touch the circle!=Коснись круга!"
        };
        
        File.WriteAllLines(Path.Combine(translationsPath, "ui.txt"), uiTranslations);
        Logger.LogInfo("Default translation files created!");
    }
    
    void CreateUntranslatedFile(string translationsPath)
    {
        string untranslatedPath = Path.Combine(translationsPath, "untranslated.txt");
        if (File.Exists(untranslatedPath))
        {
            File.Delete(untranslatedPath);
        }
        File.WriteAllText(untranslatedPath, "// Untranslated texts - copy these to appropriate translation files\n");
    }
    
    public static string GetTranslation(string original)
    {
        if (translations.TryGetValue(original, out string translated))
            return translated;
        return null;
    }
    
    public static void LogUntranslatedText(string text)
    {
        if (!loggedTexts.Contains(text))
        {
            loggedTexts.Add(text);
            
            string pluginPath = Path.GetDirectoryName(typeof(TranslationPlugin).Assembly.Location);
            string translationsPath = Path.Combine(pluginPath, "translations");
            string untranslatedPath = Path.Combine(translationsPath, "untranslated.txt");
            
            try
            {
                File.AppendAllText(untranslatedPath, $"{text}=\n");
            }
            catch (System.Exception e)
            {
                logger.LogError($"Error writing to untranslated file: {e.Message}");
            }
        }
    }
}

public static class BundleTracker
{
    public static string CurrentBundle { get; set; } = "";
    public static string LastBundlePath { get; set; } = "";
    
    public static void SetCurrentBundle(string bundleName)
    {
        if (bundleName != null && bundleName.Contains("ccscripts_assets"))
        {
            CurrentBundle = bundleName;
            TranslationPlugin.logger.LogInfo($"Current bundle set to: {CurrentBundle}");
        }
    }
    
    public static void SetLastBundlePath(string path)
    {
        if (path != null && path.Contains("ccscripts_assets"))
        {
            LastBundlePath = path;
            CurrentBundle = Path.GetFileName(path);
            TranslationPlugin.logger.LogInfo($"Bundle path: {path}, Current: {CurrentBundle}");
        }
    }
}

[HarmonyPatch]
public static class BundleLoadPatcher
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(AssetBundle), "LoadFromFile", typeof(string))]
    static void TrackAssetBundleLoad(string path)
    {
        BundleTracker.SetLastBundlePath(path);
    }
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(Resources), "Load", typeof(string))]
    static void TrackResourcesLoad(string path)
    {
        if (path.Contains("ccscripts_assets"))
        {
            BundleTracker.SetCurrentBundle(path);
        }
    }
}

[HarmonyPatch]
public static class SceneChangePatcher
{
    private static string lastSceneName = "";
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(UnityEngine.SceneManagement.SceneManager), "LoadScene", new Type[] { typeof(string), typeof(UnityEngine.SceneManagement.LoadSceneMode) })]
    static void OnSceneLoad(string sceneName, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        if (sceneName != lastSceneName)
        {
            TranslationPlugin.logger.LogInfo($"[SCENE CHANGE] {lastSceneName} -> {sceneName}");
            
            TextTranslationPatch.ResetTranslationCache();
            lastSceneName = sceneName;
            
            TranslationPlugin.logger.LogInfo($"Scene changed to: {sceneName}, cleared ALL contexts");
        }
    }
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(UnityEngine.SceneManagement.SceneManager), "LoadScene", new Type[] { typeof(int), typeof(UnityEngine.SceneManagement.LoadSceneMode) })]
    static void OnSceneLoad(int sceneBuildIndex, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        TextTranslationPatch.ResetTranslationCache();
        TranslationPlugin.logger.LogInfo($"Scene changed to index: {sceneBuildIndex}, cleared text cache");
    }
}

[HarmonyPatch]
class TextTranslationPatch
{
    private static readonly object translateLock = new object();
    
    private static void ClearAllCache()
    {
        lock (translateLock)
        {
            TranslationPlugin.logger.LogInfo($"[CLEAR ALL CACHE] Starting complete cache clear...");
            
            currentTexts.Clear();
            recentTextLog.Clear();
            
            pendingMeTranslations.Clear();
            pendingRightTranslations.Clear();
            pendingFeedsHerTranslations.Clear();
            pendingColorTranslations.Clear();
            pendingColorValues.Clear();
            socksColorSeries.Clear();
            
            colorContext = "";
            areYouOkayContext = "";
            lastLongDialogue = "";
            lastLongDialogueOriginal = "";
            wasContextUsed = false;
            processedColorsInSeries = 0;
            
            lastContextUsedTime = System.DateTime.MinValue;
            lastSocksColorTime = System.DateTime.MinValue;
            lastMeDetection = System.DateTime.MinValue;
            lastColorInSeriesTime = System.DateTime.MinValue;
            
            TranslationPlugin.logger.LogInfo($"[CLEAR ALL CACHE] Complete! All caches and contexts reset");
        }
    }
    
    public static void ResetTranslationCache()
    {
        ClearAllCache();
    }

    public static void ClearCurrentTexts()
    {
        lock (translateLock)
        {
            TranslationPlugin.logger.LogInfo($"[CLEAR TEXTS] Starting clear...");
            
            currentTexts.Clear();
            CleanupInvalidPendingTranslations();
            
            if (recentTextLog.Count > 5)
            {
                recentTextLog = recentTextLog.Skip(recentTextLog.Count - 5).ToList();
            }
            
            if (!string.IsNullOrEmpty(colorContext))
            {
                TranslationPlugin.logger.LogInfo($"[CLEAR TEXTS] Force resetting color context: {colorContext}");
                colorContext = "";
                processedColorsInSeries = 0;
                socksColorSeries.Clear();
                pendingColorTranslations.Clear();
                pendingColorValues.Clear();
                wasContextUsed = false;
            }
            
            TranslationPlugin.logger.LogInfo($"[CLEAR TEXTS] Text cache cleared");
        }
    }
    
    public static void ResetColorContext()
    {
        lock (translateLock)
        {
            TranslationPlugin.logger.LogInfo($"[RESET COLOR CONTEXT] Starting reset, current context: '{colorContext}'");
            
            colorContext = "";
            processedColorsInSeries = 0;
            socksColorSeries.Clear();
            pendingColorTranslations.Clear();
            pendingColorValues.Clear();
            
            lastContextUsedTime = System.DateTime.MinValue;
            lastSocksColorTime = System.DateTime.MinValue;
            wasContextUsed = false;
            
            TranslationPlugin.logger.LogInfo($"[RESET COLOR CONTEXT] Complete, new context: '{colorContext}'");
        }
    }
    
    private static Dictionary<int, string> currentTexts = new Dictionary<int, string>();
    private static List<string> recentTextLog = new List<string>();
    
    private static Dictionary<int, TMP_Text> pendingFeedsHerTranslations = new Dictionary<int, TMP_Text>();
    
    private static string lastLongDialogue = "";
    private static string lastLongDialogueOriginal = "";
    private static Dictionary<int, TMP_Text> pendingMeTranslations = new Dictionary<int, TMP_Text>();
    private static System.DateTime lastMeDetection = System.DateTime.MinValue;
    
    private static Dictionary<int, TMP_Text> pendingRightTranslations = new Dictionary<int, TMP_Text>();
    
    private static string colorContext = "";
    private static Dictionary<int, TMP_Text> pendingColorTranslations = new Dictionary<int, TMP_Text>();
    private static Dictionary<int, string> pendingColorValues = new Dictionary<int, string>();
    
    private static List<string> socksColorSeries = new List<string>();
    private static System.DateTime lastSocksColorTime = System.DateTime.MinValue;
    
    private static System.DateTime lastContextUsedTime = System.DateTime.MinValue;
    private static bool wasContextUsed = false;
    
    private static int processedColorsInSeries = 0;
    private static System.DateTime lastColorInSeriesTime = System.DateTime.MinValue;

    private static bool isSettingText = false;
    
    private static string areYouOkayContext = "";
    
    private static void CleanupInvalidPendingTranslations()
    {
        CleanupInvalidTMPTextDictionary(pendingMeTranslations);
        CleanupInvalidTMPTextDictionary(pendingRightTranslations);
        CleanupInvalidTMPTextDictionary(pendingFeedsHerTranslations);
        CleanupInvalidTMPTextDictionary(pendingColorTranslations);
        
        var keysToRemove = new List<int>();
        foreach (var kvp in pendingColorValues)
        {
            if (!pendingColorTranslations.ContainsKey(kvp.Key))
                keysToRemove.Add(kvp.Key);
        }
        foreach (var key in keysToRemove)
            pendingColorValues.Remove(key);
    }
    
    private static void CleanupInvalidTMPTextDictionary(Dictionary<int, TMP_Text> dictionary)
    {
        var keysToRemove = new List<int>();
        foreach (var kvp in dictionary)
        {
            if (kvp.Value == null || kvp.Value.gameObject == null)
                keysToRemove.Add(kvp.Key);
        }
        foreach (var key in keysToRemove)
            dictionary.Remove(key);
    }

    private static void CheckAndResetContext()
    {
        lock (translateLock)
        {
            if (colorContext == "socks" && socksColorSeries.Count > 0)
            {
                double timeSinceLastSocksColor = (System.DateTime.Now - lastSocksColorTime).TotalSeconds;
                
                TranslationPlugin.logger.LogInfo($"[SOCKS SERIES DEBUG] Count: {socksColorSeries.Count}, Time since last: {timeSinceLastSocksColor:F2}s, Threshold: 2.0f");
                
                if (timeSinceLastSocksColor > 2.0f && socksColorSeries.Count > 0)
                {
                    TranslationPlugin.logger.LogInfo($"[SOCKS SERIES TIMEOUT] {timeSinceLastSocksColor:F2}s passed, processing series: {string.Join(", ", socksColorSeries)}");
                    ProcessSocksColorSeries();
                }
            }

            if (wasContextUsed && !string.IsNullOrEmpty(colorContext) && 
                (System.DateTime.Now - lastContextUsedTime).TotalSeconds > 0.5f)
            {
                TranslationPlugin.logger.LogInfo($"[CONTEXT AUTO-RESET] 0.5s timeout: {colorContext}");
                colorContext = "";
                wasContextUsed = false;
                processedColorsInSeries = 0;
                socksColorSeries.Clear();
                
                if (pendingColorTranslations.Count > 0)
                {
                    TranslationPlugin.logger.LogInfo($"[CONTEXT RESET] Clearing {pendingColorTranslations.Count} pending colors");
                    pendingColorTranslations.Clear();
                    pendingColorValues.Clear();
                }
            }
        }
    }
    
    private static void ProcessSocksColor(TMP_Text component, string colorValue, int instanceId)
    {
        if (colorValue == "Green" || colorValue == "Blue" || colorValue == "Yellow")
        {
            pendingColorTranslations[instanceId] = component;
            pendingColorValues[instanceId] = colorValue;
            socksColorSeries.Add(colorValue);
            lastSocksColorTime = System.DateTime.Now;
            
            TranslationPlugin.logger.LogInfo($"{colorValue} -> PENDING for SOCKS series (ID: {instanceId})");
            TranslationPlugin.logger.LogInfo($"[SOCKS SERIES] Added {colorValue}, series: {string.Join(", ", socksColorSeries)}");
            
            currentTexts[instanceId] = colorValue;
            
            if (socksColorSeries.Count >= 3)
            {
                TranslationPlugin.logger.LogInfo($"[SOCKS SERIES] All 3 colors collected, processing immediately");
                ProcessSocksColorSeries();
            }
        }
        else
        {
            string translation = GetColorTranslation(colorValue, "socks");
            TranslationPlugin.logger.LogInfo($"{colorValue} -> {translation} (SOCKS context)");
            
            SetTranslatedText(component, translation, instanceId);
            
            processedColorsInSeries++;
            TranslationPlugin.logger.LogInfo($"[SOCKS] Processed {processedColorsInSeries}/3 colors");
            
            if (processedColorsInSeries >= 3)
            {
                TranslationPlugin.logger.LogInfo($"[CONTEXT COMPLETE] All {processedColorsInSeries} socks colors processed");
                ResetColorContext();
                
                if (component.text == colorValue)
                {
                    string defaultTranslation = GetColorTranslation(colorValue, "");
                    TranslationPlugin.logger.LogInfo($"[RECONTEXT] Re-translating {colorValue} -> {defaultTranslation} after context reset");
                    SetTranslatedText(component, defaultTranslation, instanceId);
                }
            }
        }
    }

    private static void ProcessSocksColorSeries()
    {
        if (pendingColorTranslations.Count == 0) 
        {
            TranslationPlugin.logger.LogInfo($"[PROCESS SOCKS SERIES] No pending colors to process");
            return;
        }
        
        TranslationPlugin.logger.LogInfo($"[SOCKS SERIES] Final processing: {string.Join(", ", socksColorSeries)}");
        
        bool hasBlueOrYellow = socksColorSeries.Any(c => c == "Blue" || c == "Yellow");
        bool usePlural = hasBlueOrYellow;
        
        var pendingIds = new List<int>(pendingColorTranslations.Keys);
        
        foreach (var pendingId in pendingIds)
        {
            if (!pendingColorTranslations.ContainsKey(pendingId)) continue;
                
            TMP_Text pendingText = pendingColorTranslations[pendingId];
            string pendingValue = pendingColorValues[pendingId];
            
            string translation = GetColorTranslation(pendingValue, "socks", usePlural);
            
            if (!string.IsNullOrEmpty(translation))
            {
                SetTranslatedText(pendingText, translation, pendingId);
                TranslationPlugin.logger.LogInfo($"[SOCKS SERIES] {pendingValue} -> {translation}");
            }
            
            pendingColorTranslations.Remove(pendingId);
            pendingColorValues.Remove(pendingId);
        }
        
        processedColorsInSeries += pendingIds.Count;
        
        if (processedColorsInSeries >= 3)
        {
            TranslationPlugin.logger.LogInfo($"[CONTEXT COMPLETE] All socks colors processed");
            ResetColorContext();
        }
        
        socksColorSeries.Clear();
    }

    private static string GetColorTranslation(string color, string context, bool forcePlural = false)
    {
        Dictionary<string, string> singularTranslations = new Dictionary<string, string>
        {
            ["White"] = "Белый",
            ["Black"] = "Черный",
            ["Green"] = "Зеленый",
            ["Blue"] = "Синий",
            ["Yellow"] = "Желтый",
            ["Pink"] = "Розовый",
            ["Red"] = "Красный"
        };
        
        Dictionary<string, string> pluralTranslations = new Dictionary<string, string>
        {
            ["White"] = "Белые",
            ["Black"] = "Черные",
            ["Green"] = "Зеленые",
            ["Blue"] = "Синие",
            ["Yellow"] = "Желтые",
            ["Pink"] = "Розовые",
            ["Red"] = "Красные"
        };
        
        bool usePlural = forcePlural || context == "undies" || context == "socks";
        
        if (context == "swimsuit")
            usePlural = false;
        
        if (usePlural && pluralTranslations.ContainsKey(color))
            return pluralTranslations[color];
        else if (singularTranslations.ContainsKey(color))
            return singularTranslations[color];
        
        return color;
    }

    private static void ProcessPendingColorsForContext()
    {
        lock (translateLock)
        {
            if (pendingColorTranslations.Count == 0) return;
            
            TranslationPlugin.logger.LogInfo($"Processing {pendingColorTranslations.Count} pending colors for context: {colorContext}");
            
            var pendingIds = new List<int>(pendingColorTranslations.Keys);
            
            foreach (var pendingId in pendingIds)
            {
                if (!pendingColorTranslations.ContainsKey(pendingId) || !pendingColorValues.ContainsKey(pendingId))
                    continue;
                    
                TMP_Text pendingText = pendingColorTranslations[pendingId];
                string pendingValue = pendingColorValues[pendingId];
                
                if (pendingText == null || pendingText.gameObject == null)
                {
                    pendingColorTranslations.Remove(pendingId);
                    pendingColorValues.Remove(pendingId);
                    continue;
                }
                
                string translation = GetColorTranslation(pendingValue, colorContext);
                
                if (!string.IsNullOrEmpty(translation))
                {
                    SetTranslatedText(pendingText, translation, pendingId);
                    TranslationPlugin.logger.LogInfo($"Pending {pendingValue} -> {translation} (context: {colorContext})");
                    
                    wasContextUsed = true;
                    lastContextUsedTime = System.DateTime.Now;
                }
                
                pendingColorTranslations.Remove(pendingId);
                pendingColorValues.Remove(pendingId);
            }
        }
    }
    
    private static string DetectSceneFromHistory()
    {
        lock (translateLock)
        {
            foreach (string logEntry in recentTextLog)
            {
                if (logEntry.Contains("Do you like mandarins? Want one?") || 
                    logEntry.Contains("Enjoy the tangerine") ||
                    logEntry.Contains("Gives a tangerine"))
                {
                    return "GYURI";
                }
                
                if (logEntry.Contains("See? I told you you’d need milk.") || 
                    logEntry.Contains("Ha… wow, this is really spicy.") ||
                    logEntry.Contains("Why’s it so hot?!"))
                {
                    return "YUNA";
                }
            }
            
            return "UNKNOWN";
        }
    }
    
    private static bool IsYuNaSceneMarker(string text)
    {
        return text == "Ignores them" || text == "Gives a kiss" ||
               text == "Игнорировать" || text == "Поцеловать";
    }

    private static string GetMeContext()
    {
        lock (translateLock)
        {
            string dialogueToAnalyze = lastLongDialogueOriginal;
            
            if (string.IsNullOrEmpty(dialogueToAnalyze) || 
                dialogueToAnalyze.Length < 15)
            {
                TranslationPlugin.logger.LogInfo($"[CONTEXT] Not enough dialogue context, returning UNKNOWN");
                return "UNKNOWN";
            }
            
            TranslationPlugin.logger.LogInfo($"[CONTEXT ANALYSIS] Analyzing original: '{dialogueToAnalyze}'");

            if (dialogueToAnalyze.Contains("stethoscope") || 
                dialogueToAnalyze.Contains("check if") ||
                dialogueToAnalyze.Contains("Could you help me check"))
            {
                return "STETHOSCOPE";
            }
            
            if (dialogueToAnalyze.Contains("curious about you") || 
                dialogueToAnalyze.Contains("curious about") ||
                dialogueToAnalyze.Contains("I… I’m curious about you, doctor."))
            {
                return "ABOUT_DOCTOR";
            }
            
            if (dialogueToAnalyze.Contains("you’re not here to move in") || 
                dialogueToAnalyze.Contains("move in") ||
                dialogueToAnalyze.Contains("Live here") ||
                dialogueToAnalyze.Contains("Why are you here") ||
                dialogueToAnalyze.Contains("Wait, you’re not here"))
            {
                return "MOVING_IN";
            }

            if (dialogueToAnalyze.Contains("What Are You Doing Here") ||
                dialogueToAnalyze.Contains("What are you doing here") ||
                dialogueToAnalyze.Contains("What do you want here") ||
                dialogueToAnalyze.Contains("What brings you here"))
            {
                return "PRESENCE_QUESTION";
            }
            
            if (dialogueToAnalyze.Contains("swimming") || 
                dialogueToAnalyze.Contains("athletic") ||
                dialogueToAnalyze.Contains("sport") ||
                dialogueToAnalyze.Contains("exercise") ||
                dialogueToAnalyze.Contains("work out"))
            {
                return "SPORTS";
            }
            
            if (dialogueToAnalyze.Contains("together") || 
                dialogueToAnalyze.Contains("with me") ||
                dialogueToAnalyze.Contains("with you"))
            {
                return "TOGETHER";
            }
            
            if (dialogueToAnalyze.Contains("Wanna go") || 
                dialogueToAnalyze.Contains("Want to go") ||
                dialogueToAnalyze.Contains("Let’s go") ||
                dialogueToAnalyze.Contains("How about"))
            {
                return "INVITATION";
            }
            
            if (lastLongDialogue.Contains("стетоскоп") || 
                lastLongDialogue.Contains("проверить работает") ||
                lastLongDialogue.Contains("помочь проверить"))
            {
                return "STETHOSCOPE";
            }
            
            if (lastLongDialogue.Contains("плавать") || 
                lastLongDialogue.Contains("спорт") ||
                lastLongDialogue.Contains("занимаюсь"))
            {
                return "SPORTS";
            }
            
            int checkCount = Math.Min(3, recentTextLog.Count);
            for (int i = recentTextLog.Count - checkCount; i < recentTextLog.Count; i++)
            {
                if (i < 0) continue;
                
                string recentText = recentTextLog[i];
                if (string.IsNullOrEmpty(recentText)) continue;
                
                TranslationPlugin.logger.LogInfo($"[RECENT CHECK {i}] '{recentText}'");
                
                if (recentText.Contains("stethoscope") || 
                    recentText.Contains("стетоскоп") ||
                    recentText.Contains("Could you help me check") ||
                    recentText.Contains("проверить стетоскоп"))
                {
                    TranslationPlugin.logger.LogInfo($"[HISTORY FOUND] Stethoscope in recent history");
                    return "STETHOSCOPE";
                }
                
                if (recentText.Contains("swimming") || recentText.Contains("плавать") ||
                    recentText.Contains("athletic") || recentText.Contains("спорт"))
                {
                    TranslationPlugin.logger.LogInfo($"[HISTORY FOUND] Sports in recent history");
                    return "SPORTS";
                }
                
                if (recentText.Contains("boarding house") || recentText.Contains("пансион") ||
                    recentText.Contains("share a floor") || recentText.Contains("Excuse me") ||
                    recentText.Contains("Why are you here") || recentText.Contains("Заселяться"))
                {
                    TranslationPlugin.logger.LogInfo($"[HISTORY FOUND] Moving in recent history");
                    return "MOVING_IN";
                }
                
                if (recentText.Contains("What Are You Doing Here") || 
                    recentText.Contains("What are you doing here") ||
                    recentText.Contains("What do you want here"))
                {
                    TranslationPlugin.logger.LogInfo($"[HISTORY FOUND] Presence question in recent history");
                    return "PRESENCE_QUESTION";
                }
                
                if (recentText.Contains("Wanna go") || recentText.Contains("Want to go") ||
                    recentText.Contains("together") || recentText.Contains("вместе"))
                {
                    TranslationPlugin.logger.LogInfo($"[HISTORY FOUND] Invitation in recent history");
                    return "INVITATION";
                }
                
                if (recentText.Contains("curious about") || recentText.Contains("интерес к"))
                {
                    TranslationPlugin.logger.LogInfo($"[HISTORY FOUND] Curious about in recent history");
                    return "ABOUT_DOCTOR";
                }
            }
            
            bool hasSpecificContext = dialogueToAnalyze.Contains("stethoscope") ||
                                    dialogueToAnalyze.Contains("curious") ||
                                    dialogueToAnalyze.Contains("move in") ||
                                    dialogueToAnalyze.Contains("swimming") ||
                                    dialogueToAnalyze.Contains("together") ||
                                    dialogueToAnalyze.Contains("Wanna go") ||
                                    dialogueToAnalyze.Contains("Want to go");
            
            if (hasSpecificContext)
            {
                TranslationPlugin.logger.LogInfo($"[CONTEXT DEFAULT] Has some context, defaulting to DEFAULT");
                return "DEFAULT";
            }
            else
            {
                TranslationPlugin.logger.LogInfo($"[CONTEXT] Generic question, not enough for Me? translation");
                return "UNKNOWN";
            }
        }
    }
    
    private static string GetRightContext()
    {
        lock (translateLock)
        {
            string dialogueToAnalyze = lastLongDialogueOriginal;
            
            TranslationPlugin.logger.LogInfo($"[RIGHT CONTEXT ANALYSIS] Analyzing: '{dialogueToAnalyze}'");
            
            if (dialogueToAnalyze.Contains("I can’t take a patient’s food") || 
                dialogueToAnalyze.Contains("patient’s food") ||
                dialogueToAnalyze.Contains("I can’t take"))
            {
                return "CONFIRMATION";
            }
            
            if (dialogueToAnalyze.Contains("Yuman’s type") || 
                dialogueToAnalyze.Contains("he’s mine") ||
                dialogueToAnalyze.Contains("FYI") ||
                dialogueToAnalyze.Contains("And you’re not"))
            {
                return "AGREEMENT";
            }
            
            if (dialogueToAnalyze.Contains("It’s normal for a guy") || 
                dialogueToAnalyze.Contains("like a pretty girl") ||
                dialogueToAnalyze.Contains("normal for a guy")) 
            {
                return "NORMALITY";
            }

            if (dialogueToAnalyze.Contains("No, it’s just that…") || 
                dialogueToAnalyze.Contains("We were this close to getting engaged."))
            {
                return "NORMALITY";
            }

            if (dialogueToAnalyze.Contains("This bastard’s got some serious lungs! You think you’re freakin’ Pavarotti?!"))
            {
                return "NORMALITY";
            }
            
            if (recentTextLog.Count > 0)
            {
                int startIndex = Math.Max(0, recentTextLog.Count - 10);
                for (int i = startIndex; i < recentTextLog.Count; i++)
                {
                    string recentText = recentTextLog[i];
                    
                    if (recentText.Contains("patient’s food") || recentText.Contains("take a patient"))
                    {
                        return "CONFIRMATION";
                    }
                    
                    if (recentText.Contains("Yuman") || recentText.Contains("mine") || 
                        recentText.Contains("type") || recentText.Contains("FYI"))
                    {
                        return "AGREEMENT";
                    }
                    
                    if (recentText.Contains("normal") || recentText.Contains("pretty girl") ||
                        recentText.Contains("It’s normal"))
                    {
                        return "NORMALITY";
                    }
                }
            }
            
            return "UNKNOWN";
        }
    }

    private static string GetAreYouOkayContext()
    {
        lock (translateLock)
        {
            if (!string.IsNullOrEmpty(lastLongDialogueOriginal))
            {
                string lowerDialogue = lastLongDialogueOriginal.ToLower();
                
                if (lowerDialogue.Contains("pelvis") || 
                    lowerDialogue.Contains("legs") ||
                    lowerDialogue.Contains("cross your legs") ||
                    lowerDialogue.Contains("massage") ||
                    lowerDialogue.Contains("physiotherapy") ||
                    lowerDialogue.Contains("work on") ||
                    lowerDialogue.Contains("physical therapy"))
                {
                    TranslationPlugin.logger.LogInfo($"Found MASSAGE context from last dialogue: '{lastLongDialogueOriginal}'");
                    return "MASSAGE";
                }
            }
            
            if (recentTextLog.Count > 0)
            {
                int startIndex = Math.Max(0, recentTextLog.Count - 3);
                for (int i = startIndex; i < recentTextLog.Count; i++)
                {
                    string recentText = recentTextLog[i];
                    string lowerRecent = recentText.ToLower();
                    
                    if (lowerRecent.Contains("pelvis") || 
                        lowerRecent.Contains("legs") ||
                        lowerRecent.Contains("cross your legs") ||
                        lowerRecent.Contains("massage") ||
                        lowerRecent.Contains("physiotherapy") ||
                        lowerRecent.Contains("work on") ||
                        lowerRecent.Contains("physical therapy"))
                    {
                        TranslationPlugin.logger.LogInfo($"Found MASSAGE context from recent history: '{recentText}'");
                        return "MASSAGE";
                    }
                }
            }
            
            if (recentTextLog.Count > 0)
            {
                int startIndex = Math.Max(0, recentTextLog.Count - 3);
                for (int i = startIndex; i < recentTextLog.Count; i++)
                {
                    string recentText = recentTextLog[i];

                    if (recentText.Contains("таз") || 
                        recentText.Contains("ног") ||
                        recentText.Contains("сидишь нога на ногу") ||
                        recentText.Contains("массаж") ||
                        recentText.Contains("физиотерапи") ||
                        recentText.Contains("поработаю") ||
                        recentText.Contains("работать над"))
                    {
                        TranslationPlugin.logger.LogInfo($"Found MASSAGE context from Russian translation: '{recentText}'");
                        return "MASSAGE";
                    }
                }
            }
            
            TranslationPlugin.logger.LogInfo($"No MASSAGE context found, using DEFAULT");
            return "DEFAULT";
        }
    }

    private static string GetApologyContext()
    {
        lock (translateLock)
        {
            TranslationPlugin.logger.LogInfo($"[$] GetApologyContext called, recentTextLog count: {recentTextLog.Count}");
            
            if (recentTextLog.Count < 4)
            {
                TranslationPlugin.logger.LogInfo($"[APOLOGY CONTEXT] Not enough logs: {recentTextLog.Count} < 4");
                return "UNKNOWN";
            }
            
            string dialog1 = recentTextLog[recentTextLog.Count - 4];
            string dialog2 = recentTextLog[recentTextLog.Count - 3];  
            string dialog3 = recentTextLog[recentTextLog.Count - 2];
            
            bool CheckDialogForContext(string dialog, string[] phrases)
            {
                if (string.IsNullOrEmpty(dialog)) return false;
                
                foreach (string phrase in phrases)
                {
                    if (dialog.Contains(phrase))
                        return true;
                }
                return false;
            }
            
            string[] apology1Phrases = {
                "Why didn", "Are you the reason", "You... remember",
                "Почему не сказал", "Ты причина", "Ты... помнишь"
            };
            
            string[] apology2Phrases = {
                "debt I owe", "долг я должна",
                "So basically, the story got exaggerated",
                "So basically, the story got exaggerated—",
                "because you were just cleaning up the spilled water",
                "it made it look like Min-jung was bossing people around",
                "раздули",
                "вытирала разлитую воду", 
                "будто Мин-Чжун командует"
            };
            
            string[] apology3Phrases = {
                "just to hear an apology", "чтобы услышать извинения"
            };
            
            if (CheckDialogForContext(dialog1, apology1Phrases) ||
                CheckDialogForContext(dialog2, apology1Phrases) ||
                CheckDialogForContext(dialog3, apology1Phrases))
            {
                TranslationPlugin.logger.LogInfo($"[APOLOGY CONTEXT] Found CONTEXT_APOLOGY_1!");
                return "CONTEXT_APOLOGY_1";
            }
            
            if (CheckDialogForContext(dialog1, apology2Phrases) ||
                CheckDialogForContext(dialog2, apology2Phrases) ||
                CheckDialogForContext(dialog3, apology2Phrases))
            {
                TranslationPlugin.logger.LogInfo($"[APOLOGY CONTEXT] Found CONTEXT_APOLOGY_2 (debt/after injury/story exaggerated)!");
                return "CONTEXT_APOLOGY_2";
            }
            
            if (CheckDialogForContext(dialog1, apology3Phrases) ||
                CheckDialogForContext(dialog2, apology3Phrases) ||
                CheckDialogForContext(dialog3, apology3Phrases))
            {
                TranslationPlugin.logger.LogInfo($"[APOLOGY CONTEXT] Found CONTEXT_APOLOGY_3 (saving/apology expectation)!");
                return "CONTEXT_APOLOGY_3";
            }
            
            TranslationPlugin.logger.LogInfo($"[APOLOGY CONTEXT] No context found");
            return "UNKNOWN";
        }
    }

    private static string GetRightExclamationContext()
    {
        lock (translateLock)
        {
            if (recentTextLog.Count < 3)
            {
                return "UNKNOWN";
            }
            
            string[] lastThree = new string[3];
            int startIndex = Math.Max(0, recentTextLog.Count - 3);
            for (int i = 0; i < 3 && (startIndex + i) < recentTextLog.Count; i++)
            {
                lastThree[i] = recentTextLog[startIndex + i];
            }
            
            TranslationPlugin.logger.LogInfo($"[RIGHT?! CONTEXT] Last 3: '{string.Join(" | ", lastThree)}'");
            
            foreach (string dialog in lastThree)
            {
                if (dialog == null) continue;
                
                if (dialog.Contains("I’m kind of picky") ||
                    dialog.Contains("я достаточно привередлива") ||
                    dialog.Contains("just drink it already") ||
                    dialog.Contains("давай уже пей") ||
                    dialog.Contains("this is actually good") ||
                    dialog.Contains("это на самом деле вкусно"))
                {
                    TranslationPlugin.logger.LogInfo($"[RIGHT?! CONTEXT] Found CONTEXT_RIGHT_EXCLAMATION in: {dialog}");
                    return "CONTEXT_RIGHT_EXCLAMATION";
                }
            }
            
            return "UNKNOWN";
        }
    }
    
    private static string GetHappyNowContext()
    {
        lock (translateLock)
        {
            TranslationPlugin.logger.LogInfo($"[HAPPY NOW CONTEXT] Analyzing recent logs, count: {recentTextLog.Count}");
            
            if (recentTextLog.Count < 4)
            {
                TranslationPlugin.logger.LogInfo($"[HAPPY NOW CONTEXT] Not enough logs: {recentTextLog.Count} < 4");
                return "UNKNOWN";
            }
            
            int startIndex = Math.Max(0, recentTextLog.Count - 6);
            int endIndex = recentTextLog.Count - 2;
            
            TranslationPlugin.logger.LogInfo($"[HAPPY NOW CONTEXT] Checking dialogs from {startIndex} to {endIndex}:");
            
            int moreCount = 0;
            bool hasExclamation = false;
            
            for (int i = startIndex; i <= endIndex; i++)
            {
                string dialog = recentTextLog[i];
                if (dialog == null) continue;
                
                TranslationPlugin.logger.LogInfo($"  [{i}]: '{dialog}'");
                
                if (dialog == "More." || dialog == "Еще.")
                {
                    moreCount++;
                }
                else if (dialog == "More. More." || dialog == "Еще. Еще.")
                {
                    moreCount += 2;
                }
                else if (dialog == "More. More. More. More!" || dialog == "Еще. Еще.Еще. Еще!")
                {
                    moreCount += 4;
                    hasExclamation = true;
                }
            }
            
            TranslationPlugin.logger.LogInfo($"[HAPPY NOW CONTEXT] Found 'More.' count: {moreCount}, hasExclamation: {hasExclamation}");
            
            if (moreCount >= 2 && hasExclamation)
            {
                TranslationPlugin.logger.LogInfo($"[HAPPY NOW CONTEXT] Found CONTEXT_HAPPY_NOW_MORE (multiple More with exclamation)!");
                return "CONTEXT_HAPPY_NOW_MORE";
            }
            else if (moreCount >= 2)
            {
                TranslationPlugin.logger.LogInfo($"[HAPPY NOW CONTEXT] Found CONTEXT_HAPPY_NOW_MORE (multiple More)!");
                return "CONTEXT_HAPPY_NOW_MORE";
            }
            
            TranslationPlugin.logger.LogInfo($"[HAPPY NOW CONTEXT] No context found");
            return "UNKNOWN";
        }
    }

    private static string GetWhyContext()
    {
        lock (translateLock)
        {
            TranslationPlugin.logger.LogInfo($"[WHY CONTEXT] Analyzing recent logs, count: {recentTextLog.Count}");
            
            if (recentTextLog.Count < 3)
            {
                TranslationPlugin.logger.LogInfo($"[WHY CONTEXT] Not enough logs: {recentTextLog.Count} < 3");
                return "UNKNOWN";
            }
            
            int startIndex = Math.Max(0, recentTextLog.Count - 5);
            int endIndex = recentTextLog.Count - 2;
            
            TranslationPlugin.logger.LogInfo($"[WHY CONTEXT] Checking dialogs from {startIndex} to {endIndex}:");
            
            bool hasActionRequest = false;
            bool hasGetInMyArmsandBathroom = false;
            bool hasDrinkingContext = false;
            bool hasChiefContext = false;
            
            for (int i = startIndex; i <= endIndex; i++)
            {
                string dialog = recentTextLog[i];
                if (dialog == null) continue;
                
                TranslationPlugin.logger.LogInfo($"  [{i}]: '{dialog}'");
                
                if (dialog == "Hey, can you stand up for a sec?" || 
                    dialog == "Эй, можешь встать на секунду?" ||
                    dialog.Contains("stand up for a sec") ||
                    dialog.Contains("встать на секунду") ||
                    dialog == "Can you turn around for a sec?" ||
                    dialog == "Можешь повернуться на секунду?" ||
                    dialog.Contains("turn around for a sec") ||
                    dialog.Contains("повернуться на секунду"))
                {
                    hasActionRequest = true;
                    TranslationPlugin.logger.LogInfo($"[WHY CONTEXT] Found action request: '{dialog}'");
                }
                
                if (dialog == "Get in my arms." ||
                    dialog == "Иди в мои объятия.")
                {
                    hasGetInMyArmsandBathroom = true;
                    TranslationPlugin.logger.LogInfo($"[WHY CONTEXT] Found 'Get in my arms': '{dialog}'");
                }

                if (dialog == "Where are you going?" ||
                    dialog == "Huh? I’m going to the bathroom…")
                {
                    hasGetInMyArmsandBathroom = true;
                    TranslationPlugin.logger.LogInfo($"[WHY CONTEXT] Found 'bathroom': '{dialog}'");
                }

                if (dialog == "Alright… shall we start drinking?" ||
                    dialog == "Ну что… пора выпить?" ||
                    dialog.Contains("shall we start drinking") ||
                    dialog.Contains("пора выпить"))
                {
                    hasDrinkingContext = true;
                    TranslationPlugin.logger.LogInfo($"[WHY CONTEXT] Found drinking context: '{dialog}'");
                }

                if (dialog == "Chief, are you still awake?" ||
                    dialog == "Шеф, ты еще не спишь?")
                {
                    hasChiefContext = true;
                    TranslationPlugin.logger.LogInfo($"[WHY CONTEXT] Found Chief context: '{dialog}'");
                }
            }
            
            if (hasGetInMyArmsandBathroom)
            {
                TranslationPlugin.logger.LogInfo($"[WHY CONTEXT] Found CONTEXT_WHY_GET_IN_MY_ARMS_and_BATHROOM!");
                return "CONTEXT_WHY_GET_IN_MY_ARMS_and_BATHROOM";
            }
            else if (hasActionRequest)
            {
                TranslationPlugin.logger.LogInfo($"[WHY CONTEXT] Found CONTEXT_WHY_ACTION_REQUEST!");
                return "CONTEXT_WHY_ACTION_REQUEST";
            }
            else if (hasDrinkingContext)
            {
                TranslationPlugin.logger.LogInfo($"[WHY CONTEXT] Found CONTEXT_WHY_DRINKING!");
                return "CONTEXT_WHY_DRINKING";
            }
            else if (hasChiefContext)
            {
                TranslationPlugin.logger.LogInfo($"[WHY CONTEXT] Found CONTEXT_WHY_CHIEF!");
                return "CONTEXT_WHY_CHIEF";
            }
            
            TranslationPlugin.logger.LogInfo($"[WHY CONTEXT] No context found");
            return "UNKNOWN";
        }
    }

    private static string GetHoldOnContext()
    {
        lock (translateLock)
        {
            TranslationPlugin.logger.LogInfo($"[HOLD ON CONTEXT] Analyzing recent logs, count: {recentTextLog.Count}");
            
            if (recentTextLog.Count < 3)
            {
                TranslationPlugin.logger.LogInfo($"[HOLD ON CONTEXT] Not enough logs: {recentTextLog.Count} < 3");
                return "UNKNOWN";
            }
            
            int startIndex = Math.Max(0, recentTextLog.Count - 5);
            int endIndex = recentTextLog.Count - 2;
            
            TranslationPlugin.logger.LogInfo($"[HOLD ON CONTEXT] Checking dialogs from {startIndex} to {endIndex}:");
            
            bool hasHairLoose = false;
            
            for (int i = startIndex; i <= endIndex; i++)
            {
                string dialog = recentTextLog[i];
                if (dialog == null) continue;
                
                TranslationPlugin.logger.LogInfo($"  [{i}]: '{dialog}'");
                
                if (dialog == "Your hair came loose." || 
                    dialog.Contains("hair came loose") ||
                    dialog.Contains("волосы распустились"))
                {
                    hasHairLoose = true;
                    TranslationPlugin.logger.LogInfo($"[HOLD ON CONTEXT] Found hair loose mention");
                }
            }
            
            if (hasHairLoose)
            {
                TranslationPlugin.logger.LogInfo($"[HOLD ON CONTEXT] Found CONTEXT_HOLD_ON_HAIR_LOOSE!");
                return "CONTEXT_HOLD_ON_HAIR_LOOSE";
            }
            
            TranslationPlugin.logger.LogInfo($"[HOLD ON CONTEXT] No context found");
            return "UNKNOWN";
        }
    }
    
    private static string GetArmContext()
    {
        lock (translateLock)
        {
            TranslationPlugin.logger.LogInfo($"[ARM CONTEXT] Analyzing recent logs, count: {recentTextLog.Count}");
            
            if (recentTextLog.Count < 3)
            {
                TranslationPlugin.logger.LogInfo($"[ARM CONTEXT] Not enough logs: {recentTextLog.Count} < 3");
                return "UNKNOWN";
            }
            
            int startIndex = Math.Max(0, recentTextLog.Count - 5);
            int endIndex = recentTextLog.Count - 2;
            
            TranslationPlugin.logger.LogInfo($"[ARM CONTEXT] Checking dialogs from {startIndex} to {endIndex}:");
            
            bool hasBodyPartsContext = false;
            
            for (int i = startIndex; i <= endIndex; i++)
            {
                string dialog = recentTextLog[i];
                if (dialog == null) continue;
                
                TranslationPlugin.logger.LogInfo($"  [{i}]: '{dialog}'");
                
                if (dialog == "Head" || dialog == "Голова" ||
                    dialog == "Knee" || dialog == "Колени")
                {
                    hasBodyPartsContext = true;
                    TranslationPlugin.logger.LogInfo($"[ARM CONTEXT] Found body parts context: '{dialog}'");
                }
            }
            
            if (hasBodyPartsContext)
            {
                TranslationPlugin.logger.LogInfo($"[ARM CONTEXT] Found CONTEXT_ARM_BODY_PARTS!");
                return "CONTEXT_ARM_BODY_PARTS";
            }
            
            TranslationPlugin.logger.LogInfo($"[ARM CONTEXT] No context found");
            return "UNKNOWN";
        }
    }
    
    private static void ResetAreYouOkayContext()
    {
        lock (translateLock)
        {
            areYouOkayContext = "";
            TranslationPlugin.logger.LogInfo($"Are you okay? context reset");
        }
    }
    
    private static string GetUghSeriouslyContext()
    {
        lock (translateLock)
        {
            if (recentTextLog.Count < 2)
            {
                return "UNKNOWN";
            }

            string previousDialogue = recentTextLog[recentTextLog.Count - 2];

            TranslationPlugin.logger.LogInfo($"[UGH CONTEXT] Previous dialogue: '{previousDialogue}'");

            if (previousDialogue != null && 
                (previousDialogue.Contains("used to fit you loose") || 
                previousDialogue.Contains("болталось") ||
                previousDialogue.Contains("fit you loose") ||
                previousDialogue.Contains("then,") && previousDialogue.Contains("fit")))
            {
                TranslationPlugin.logger.LogInfo($"[UGH CONTEXT] Found CONTEXT_FIT_LOOSE!");
                return "CONTEXT_FIT_LOOSE";
            }

            if (!string.IsNullOrEmpty(lastLongDialogueOriginal) && 
                (lastLongDialogueOriginal.Contains("used to fit you loose") || 
                lastLongDialogueOriginal.Contains("fit you loose")))
            {
                TranslationPlugin.logger.LogInfo($"[UGH CONTEXT] Found CONTEXT_FIT_LOOSE in lastLongDialogueOriginal!");
                return "CONTEXT_FIT_LOOSE";
            }

            TranslationPlugin.logger.LogInfo($"[UGH CONTEXT] No specific context found");
            return "UNKNOWN";
        }
    }

    private static string GetHuhMeContext()
    {
        lock (translateLock)
        {
            TranslationPlugin.logger.LogInfo($"[HUH ME CONTEXT] Analyzing context...");
            
            for (int i = Math.Max(0, recentTextLog.Count - 3); i < recentTextLog.Count; i++)
            {
                string dialog = recentTextLog[i];
                if (dialog == null) continue;
                
                TranslationPlugin.logger.LogInfo($"  Recent [{i}]: '{dialog}'");
                
                if (dialog.Contains("whenever I see you") || 
                    dialog.Contains("когда я тебя вижу"))
                {
                    TranslationPlugin.logger.LogInfo($"[HUH ME CONTEXT] Found CONTEXT_SEE_YOU!");
                    return "CONTEXT_SEE_YOU";
                }
                
                if (dialog == "Wanna give it a try, Yuman?" ||
                    dialog == "Хочешь попробовать, Юман?" ||
                    dialog.Contains("give it a try") ||
                    dialog.Contains("попробовать") ||
                    dialog.Contains("put this on") ||
                    dialog.Contains("наносить лак"))
                {
                    TranslationPlugin.logger.LogInfo($"[HUH ME CONTEXT] Found CONTEXT_TRY_NAIL_POLISH!");
                    return "CONTEXT_TRY_NAIL_POLISH";
                }
            }
            
            TranslationPlugin.logger.LogInfo($"[HUH ME CONTEXT] No specific context found");
            return "UNKNOWN";
        }
    }

    private static string GetHuhWhyContext()
    {
        lock (translateLock)
        {
            TranslationPlugin.logger.LogInfo($"[HUH WHY CONTEXT] Analyzing recent logs, count: {recentTextLog.Count}");
            
            if (recentTextLog.Count < 3)
            {
                TranslationPlugin.logger.LogInfo($"[HUH WHY CONTEXT] Not enough logs: {recentTextLog.Count} < 3");
                return "UNKNOWN";
            }
            
            int startIndex = Math.Max(0, recentTextLog.Count - 5);
            int endIndex = recentTextLog.Count - 2;
            
            TranslationPlugin.logger.LogInfo($"[HUH WHY CONTEXT] Checking dialogs from {startIndex} to {endIndex}:");
            
            bool hasSmoothGuyContext = false;
            
            for (int i = startIndex; i <= endIndex; i++)
            {
                string dialog = recentTextLog[i];
                if (dialog == null) continue;
                
                TranslationPlugin.logger.LogInfo($"  [{i}]: '{dialog}'");
                
                if (dialog == "Even if a guy acted too smooth… I think I’d hate that." ||
                    dialog == "Даже если парень будет вести себя слишком гладко… думаю, мне бы это не понравилось." ||
                    dialog.Contains("acted too smooth") ||
                    dialog.Contains("ведет себя слишком гладко") ||
                    dialog.Contains("smooth guy") ||
                    dialog.Contains("гладкий парень"))
                {
                    hasSmoothGuyContext = true;
                    TranslationPlugin.logger.LogInfo($"[HUH WHY CONTEXT] Found 'smooth guy' context: '{dialog}'");
                }
            }
            
            if (hasSmoothGuyContext)
            {
                TranslationPlugin.logger.LogInfo($"[HUH WHY CONTEXT] Found CONTEXT_HUH_WHY_SMOOTH_GUY!");
                return "CONTEXT_HUH_WHY_SMOOTH_GUY";
            }
            
            TranslationPlugin.logger.LogInfo($"[HUH WHY CONTEXT] No context found");
            return "UNKNOWN";
        }
    }

    private static string GetAhContext()
    {
        lock (translateLock)
        {
            TranslationPlugin.logger.LogInfo($"[AH CONTEXT] Analyzing recent logs, count: {recentTextLog.Count}");
            
            if (recentTextLog.Count < 3)
            {
                TranslationPlugin.logger.LogInfo($"[AH CONTEXT] Not enough logs: {recentTextLog.Count} < 3");
                return "UNKNOWN";
            }
            
            int startIndex = Math.Max(0, recentTextLog.Count - 5);
            int endIndex = recentTextLog.Count - 2;
            
            TranslationPlugin.logger.LogInfo($"[AH CONTEXT] Checking dialogs from {startIndex} to {endIndex}:");
            
            bool hasMedicalContext = false;
            
            for (int i = startIndex; i <= endIndex; i++)
            {
                string dialog = recentTextLog[i];
                if (dialog == null) continue;
                
                TranslationPlugin.logger.LogInfo($"  [{i}]: '{dialog}'");
                
                if (dialog == "Okay, say “ah~”" ||
                    dialog == "Ладно, скажи “а~”" ||
                    dialog == "Can you cut it a little smaller…?" ||
                    dialog == "Можно порезать поменьше…?" ||
                    dialog == "Just eat it~" ||
                    dialog == "Просто ешь~" ||
                    dialog == "I’m the patient here…" ||
                    dialog == "Я же пациент…" ||
                    dialog == "Ugh, my arm hurts." ||
                    dialog == "Уф, рука болит." ||
                    dialog == "P-please~" ||
                    dialog == "П-пожалуйста~" ||
                    dialog == "Say “ah~”" ||
                    dialog == "Скажи “а~”" ||
                    dialog == "Here, say “ah~”" ||
                    dialog == "Вот, скажи “а~”" ||
                    dialog == "Oppa, say “ah~”" ||
                    dialog == "Оппа, скажи “а~”" ||
                    dialog == "Yes! More, please! More!" ||
                    dialog == "Да! Еще, пожалуйста! Еще!" ||
                    dialog == "Now come on—say “ah~”!" ||
                    dialog == "Ну же — скажи “а~”!" ||
                    dialog == "Well, I have been training to be a perfect bride since I was little. Want more?" ||
                    dialog == "Ну, меня с детства готовили быть идеальной невестой. Еще хочешь?" ||
                    dialog == "Yeah, this white stuff here…" ||
                    dialog == "Да, вот эту белую штуку…" ||
                    dialog == "Hurry up and try it too." ||
                    dialog == "Давай, попробуй скорее тоже." ||
                    dialog == "Yuman! Say ah~" ||
                    dialog == "Юман! Скажи “аа~”")
                {
                    hasMedicalContext = true;
                    TranslationPlugin.logger.LogInfo($"[AH CONTEXT] Found medical context: '{dialog}'");
                }
            }
            
            if (hasMedicalContext)
            {
                TranslationPlugin.logger.LogInfo($"[AH CONTEXT] Found CONTEXT_AH_FOOD!");
                return "CONTEXT_AH_FOOD";
            }
            
            TranslationPlugin.logger.LogInfo($"[AH CONTEXT] No context found");
            return "UNKNOWN";
        }
    }

    private static string GetTalkContext()
    {
        lock (translateLock)
        {
            TranslationPlugin.logger.LogInfo($"[TALK CONTEXT] Analyzing recent logs, count: {recentTextLog.Count}");
            
            if (recentTextLog.Count < 3)
            {
                TranslationPlugin.logger.LogInfo($"[TALK CONTEXT] Not enough logs: {recentTextLog.Count} < 3");
                return "UNKNOWN";
            }
            
            int startIndex = Math.Max(0, recentTextLog.Count - 5);
            int endIndex = Math.Min(recentTextLog.Count - 1, recentTextLog.Count);
            
            TranslationPlugin.logger.LogInfo($"[TALK CONTEXT] Checking dialogs from {startIndex} to {endIndex}:");
            
            bool hasRunAwayOption = false;
            
            for (int i = startIndex; i < endIndex; i++)
            {
                string dialog = recentTextLog[i];
                if (dialog == null) continue;
                
                TranslationPlugin.logger.LogInfo($"  [{i}]: '{dialog}'");
                
                if (dialog == "Run Away" || dialog == "Убежать")
                {
                    hasRunAwayOption = true;
                    TranslationPlugin.logger.LogInfo($"[TALK CONTEXT] Found 'Run Away' option: '{dialog}'");
                    break;
                }
            }
            
            if (hasRunAwayOption)
            {
                TranslationPlugin.logger.LogInfo($"[TALK CONTEXT] Found CONTEXT_TALK_ACTION_CHOICE!");
                return "CONTEXT_TALK_ACTION_CHOICE";
            }
            
            TranslationPlugin.logger.LogInfo($"[TALK CONTEXT] No context found");
            return "UNKNOWN";
        }
    }

    private static string GetWhatIsItContext()
    {
        lock (translateLock)
        {
            TranslationPlugin.logger.LogInfo($"[WHAT IS IT CONTEXT] Analyzing recent logs, count: {recentTextLog.Count}");
            
            for (int i = Math.Max(0, recentTextLog.Count - 3); i < recentTextLog.Count; i++)
            {
                string dialog = recentTextLog[i];
                
                if (dialog == "Ah—jeez, you scared me!" || 
                    dialog == "Ах—боже, ты напугал меня!" ||
                    dialog.Contains("scared me") ||
                    dialog.Contains("напугал"))
                {
                    TranslationPlugin.logger.LogInfo($"[WHAT IS IT CONTEXT] Found 'scared me' at index {i}, using CONTEXT_WHAT_IS_IT_SCARED");
                    return "CONTEXT_WHAT_IS_IT_SCARED";
                }
                
                if (dialog == "Wanna know my secret move for when I get scared?" ||
                    dialog == "Хочешь узнать мой секретный прием, когда страшно?" ||
                    dialog.Contains("secret move") ||
                    dialog.Contains("секретный прием"))
                {
                    TranslationPlugin.logger.LogInfo($"[WHAT IS IT CONTEXT] Found 'secret move' at index {i}, using CONTEXT_WHAT_IS_IT_SECRET_MOVE");
                    return "CONTEXT_WHAT_IS_IT_SECRET_MOVE";
                }
            }
            
            TranslationPlugin.logger.LogInfo($"[WHAT IS IT CONTEXT] No context found");
            return "UNKNOWN";
        }
    }

    private static string GetYouSureContext()
    {
        lock (translateLock)
        {
            TranslationPlugin.logger.LogInfo($"[YOU SURE CONTEXT] Analyzing recent logs, count: {recentTextLog.Count}");
            
            for (int i = Math.Max(0, recentTextLog.Count - 3); i < recentTextLog.Count; i++)
            {
                string dialog = recentTextLog[i];
                if (dialog == "No—just… turn around for a sec." || 
                    dialog == "Нет—просто… отвернись на секунду." ||
                    dialog == "I’ll get up on my own." ||
                    dialog == "Я сама встану.")
                {
                    TranslationPlugin.logger.LogInfo($"[YOU SURE CONTEXT] Found female context at index {i}, using CONTEXT_YOU_SURE_FEMALE");
                    return "CONTEXT_YOU_SURE_FEMALE";
                }
            }
            
            TranslationPlugin.logger.LogInfo($"[YOU SURE CONTEXT] No context found");
            return "UNKNOWN";
        }
    }

    private static string GetAhSorryContext()
    {
        lock (translateLock)
        {
            TranslationPlugin.logger.LogInfo($"[AH SORRY CONTEXT] Analyzing recent logs, count: {recentTextLog.Count}");
            
            for (int i = Math.Max(0, recentTextLog.Count - 3); i < recentTextLog.Count; i++)
            {
                string dialog = recentTextLog[i];
                if (dialog == "Uh… I think I can let go now." || 
                    dialog == "Э-э… Думаю, теперь можно отпустить.")
                {
                    TranslationPlugin.logger.LogInfo($"[AH SORRY CONTEXT] Found 'let go' context at index {i}, using CONTEXT_AH_SORRY_LET_GO");
                    return "CONTEXT_AH_SORRY_LET_GO";
                }
            }
            
            TranslationPlugin.logger.LogInfo($"[AH SORRY CONTEXT] No context found");
            return "UNKNOWN";
        }
    }

    private static string GetWhiteContext()
    {
        lock (translateLock)
        {
            TranslationPlugin.logger.LogInfo($"[WHITE CONTEXT] Analyzing recent logs, count: {recentTextLog.Count}");
            
            for (int i = Math.Max(0, recentTextLog.Count - 3); i < recentTextLog.Count; i++)
            {
                string dialog = recentTextLog[i];
                if (dialog == "You were wearing white." || 
                    dialog == "На тебе был белый." ||
                    dialog.Contains("were wearing white") ||
                    dialog.Contains("был белый"))
                {
                    TranslationPlugin.logger.LogInfo($"[WHITE CONTEXT] Found 'wearing white' at index {i}, using CONTEXT_WHITE_SINGULAR");
                    return "CONTEXT_WHITE_SINGULAR";
                }
            }
            
            TranslationPlugin.logger.LogInfo($"[WHITE CONTEXT] No context found");
            return "UNKNOWN";
        }
    }

    private static string GetSoundsGoodContext()
    {
        lock (translateLock)
        {
            TranslationPlugin.logger.LogInfo($"[SOUNDS GOOD CONTEXT] Analyzing recent logs, count: {recentTextLog.Count}");
            
            for (int i = Math.Max(0, recentTextLog.Count - 3); i < recentTextLog.Count; i++)
            {
                string dialog = recentTextLog[i];
                if (dialog == "There you all are." || 
                    dialog == "Are you two having fun without us?" ||
                    dialog == "Shall we head in too?" ||
                    dialog == "А, вот и вы все." ||
                    dialog == "А вы вдвоем развлекаетесь без нас?" ||
                    dialog == "Может, и нам зайти?")
                {
                    TranslationPlugin.logger.LogInfo($"[SOUNDS GOOD CONTEXT] Found group/entry context at index {i}, using CONTEXT_SOUNDS_GOOD_GROUP_ENTRY");
                    return "CONTEXT_SOUNDS_GOOD_GROUP_ENTRY";
                }
            }
            
            TranslationPlugin.logger.LogInfo($"[SOUNDS GOOD CONTEXT] No context found");
            return "UNKNOWN";
        }
    }

    private static string GetTakeALookContext()
    {
        lock (translateLock)
        {
            TranslationPlugin.logger.LogInfo($"[TAKE A LOOK CONTEXT] Analyzing recent logs, count: {recentTextLog.Count}");
            
            // Проверяем, были ли недавно фразы про фотографии/результат
            for (int i = Math.Max(0, recentTextLog.Count - 3); i < recentTextLog.Count; i++)
            {
                string dialog = recentTextLog[i];
                if (dialog == "Oh wow, these turned out great!" || 
                    dialog == "О, вау, получилось здорово!" ||
                    dialog.Contains("turned out great") ||
                    dialog.Contains("получилось здорово") ||
                    dialog.Contains("Hehe, really?") ||
                    dialog.Contains("Хе-хе, правда?"))
                {
                    TranslationPlugin.logger.LogInfo($"[TAKE A LOOK CONTEXT] Found photo result context at index {i}, using CONTEXT_TAKE_A_LOOK_PHOTO");
                    return "CONTEXT_TAKE_A_LOOK_PHOTO";
                }
            }
            
            TranslationPlugin.logger.LogInfo($"[TAKE A LOOK CONTEXT] No context found");
            return "UNKNOWN";
        }
    }

    private static string GetComingContext()
    {
        lock (translateLock)
        {
            TranslationPlugin.logger.LogInfo($"[COMING CONTEXT] Analyzing recent logs, count: {recentTextLog.Count}");
            
            for (int i = Math.Max(0, recentTextLog.Count - 3); i < recentTextLog.Count; i++)
            {
                string dialog = recentTextLog[i];
                if (dialog == "Yu-jeong! Can you grab my socks?!" || 
                    dialog == "Ючжон! Подашь мне носки?!")
                {
                    TranslationPlugin.logger.LogInfo($"[COMING CONTEXT] Found socks context at index {i}, using CONTEXT_COMING_SOCKS");
                    return "CONTEXT_COMING_SOCKS";
                }
            }
            
            TranslationPlugin.logger.LogInfo($"[COMING CONTEXT] No context found");
            return "UNKNOWN";
        }
    }

    private static string GetHelloContext()
    {
        lock (translateLock)
        {
            TranslationPlugin.logger.LogInfo($"[HELLO CONTEXT] Analyzing recent logs, count: {recentTextLog.Count}");
            
            // Проверяем, была ли недавно фраза "Wuseon, hey!"
            for (int i = Math.Max(0, recentTextLog.Count - 3); i < recentTextLog.Count; i++)
            {
                string dialog = recentTextLog[i];
                if (dialog == "Oh! Wuseon, hey!" || 
                    dialog == "О! У Сон, привет!")
                {
                    TranslationPlugin.logger.LogInfo($"[HELLO CONTEXT] Found Wuseon context at index {i}, using CONTEXT_HELLO_WUSEON");
                    return "CONTEXT_HELLO_WUSEON";
                }
            }
            
            TranslationPlugin.logger.LogInfo($"[HELLO CONTEXT] No context found");
            return "UNKNOWN";
        }
    }

    private static void ProcessPendingFeedsHer(string context)
    {
        lock (translateLock)
        {
            if (pendingFeedsHerTranslations.Count == 0) return;
            
            TranslationPlugin.logger.LogInfo($"Processing {pendingFeedsHerTranslations.Count} pending Feeds Her translations...");
            
            string translation = context == "YUNA" ? "Напоить ее" : "Покормить/Напоить";
            
            foreach (var pending in pendingFeedsHerTranslations.ToList())
            {
                int pendingId = pending.Key;
                TMP_Text pendingText = pending.Value;
                
                if (pendingText != null && pendingText.gameObject != null)
                {
                    pendingText.text = translation;
                    currentTexts[pendingId] = translation;
                    
                    TranslationPlugin.logger.LogInfo($"Pending Feeds Her (ID: {pendingId}) -> {translation}");
                }
                
                pendingFeedsHerTranslations.Remove(pendingId);
            }
        }
    }
    
    private static void ProcessPendingMe(string context)
    {
        lock (translateLock)
        {
            if (pendingMeTranslations.Count == 0) return;
            
            TranslationPlugin.logger.LogInfo($"Processing {pendingMeTranslations.Count} pending Me? translations...");
            
            string translation;
            if (context == "ABOUT_DOCTOR" || context == "PRESENCE_QUESTION")
            {
                translation = "Обо мне?";
            }
            else
            {
                translation = "Я?";
            }
            
            foreach (var pending in pendingMeTranslations.ToList())
            {
                int pendingId = pending.Key;
                TMP_Text pendingText = pending.Value;
                
                if (pendingText != null && pendingText.gameObject != null)
                {
                    pendingText.text = translation;
                    currentTexts[pendingId] = translation;
                    
                    TranslationPlugin.logger.LogInfo($"Pending Me? (ID: {pendingId}) -> {translation} (context: {context})");
                }
                
                pendingMeTranslations.Remove(pendingId);
            }
        }
    }

    private static void ProcessPendingMeWithDefault()
    {
        lock (translateLock)
        {
            if (pendingMeTranslations.Count == 0) return;
            
            TranslationPlugin.logger.LogInfo($"Processing {pendingMeTranslations.Count} pending Me? with DEFAULT context...");
            
            foreach (var pending in pendingMeTranslations.ToList())
            {
                int pendingId = pending.Key;
                TMP_Text pendingText = pending.Value;
                
                if (pendingText != null && pendingText.gameObject != null)
                {
                    SetTranslatedText(pendingText, "Я?", pendingId);
                    
                    TranslationPlugin.logger.LogInfo($"Pending Me? (ID: {pendingId}) -> Я? (default fallback)");
                }
                
                pendingMeTranslations.Remove(pendingId);
            }
        }
    }
    
    private static void ProcessPendingRight(string context)
    {
        lock (translateLock)
        {
            if (pendingRightTranslations.Count == 0) return;
            
            TranslationPlugin.logger.LogInfo($"Processing {pendingRightTranslations.Count} pending Right? translations...");
            
            string translation;
            switch (context)
            {
                case "CONFIRMATION":
                    translation = "Точно?";
                    break;
                case "AGREEMENT":
                    translation = "Да?";
                    break;
                case "NORMALITY":
                    translation = "Верно?";
                    break;
                default:
                    translation = "Правильно?";
                    break;
            }
            
            foreach (var pending in pendingRightTranslations.ToList())
            {
                int pendingId = pending.Key;
                TMP_Text pendingText = pending.Value;
                
                if (pendingText != null && pendingText.gameObject != null)
                {
                    pendingText.text = translation;
                    currentTexts[pendingId] = translation;
                    
                    TranslationPlugin.logger.LogInfo($"Pending Right? (ID: {pendingId}) -> {translation} (context: {context})");
                }
                
                pendingRightTranslations.Remove(pendingId);
            }
        }
    }
    
    private static void ClearPendingTranslations()
    {
        lock (translateLock)
        {
            ProcessPendingMeWithDefault();
            
            if (pendingRightTranslations.Count > 0)
            {
                TranslationPlugin.logger.LogInfo($"Clearing {pendingRightTranslations.Count} pending Right? translations");
                pendingRightTranslations.Clear();
            }
            
            if (pendingFeedsHerTranslations.Count > 0)
            {
                TranslationPlugin.logger.LogInfo($"Clearing {pendingFeedsHerTranslations.Count} pending Feeds Her translations");
                pendingFeedsHerTranslations.Clear();
            }
            
            if (pendingColorTranslations.Count > 0)
            {
                TranslationPlugin.logger.LogInfo($"Clearing {pendingColorTranslations.Count} pending color translations");
                pendingColorTranslations.Clear();
                pendingColorValues.Clear();
            }
            
            lastLongDialogue = "";
            lastLongDialogueOriginal = "";

            ResetAreYouOkayContext();
            
            colorContext = "";
            socksColorSeries.Clear();
            recentTextLog.Clear();
            wasContextUsed = false;
            processedColorsInSeries = 0;
        }
    }
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(Text), "set_text")]
    static void TranslateUIText(Text __instance, string value)
    {
        if (value != null && value.Contains("UnityExplorer"))
            return;

        if (__instance == null || __instance.gameObject == null || string.IsNullOrEmpty(value)) 
            return;
        
        lock (translateLock)
        {
            try
            {
                int instanceId = __instance.GetInstanceID();
                
                if (currentTexts.TryGetValue(instanceId, out string currentText) && currentText == value)
                    return;
                    
                Action<string> setText = (newText) => 
                {
                    try
                    {
                        if (__instance == null || __instance.gameObject == null) return;
                        __instance.text = newText;
                        currentTexts[instanceId] = newText;
                    }
                    catch (Exception e)
                    {
                        TranslationPlugin.logger.LogError($"Error in UI setText: {e}");
                    }
                };
                
                ProcessText(__instance.gameObject, value, instanceId, setText);
            }
            catch (Exception e)
            {
                TranslationPlugin.logger.LogError($"Error in TranslateUIText: {e}");
            }
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(TMP_Text), "set_text")]
    static void TranslateTMPText(TMP_Text __instance, string value)
    {
        if (value != null && value.Contains("UnityExplorer"))
            return;

        if (__instance == null || __instance.gameObject == null || string.IsNullOrEmpty(value)) 
            return;
        
        if (isSettingText)
            return;

        CheckAndResetContext();

        // 1.
        if (value == "Touch the circle to the border!" || value == "Touch the circle!")
        {
            TranslationPlugin.logger.LogInfo($"DIRECT CATCH: Touch phrase detected: '{value}'");
            
            string translated = TranslationPlugin.GetTranslation(value);
            if (translated == null)
            {
                translated = value == "Touch the circle to the border!" ? 
                    "Коснись круга внутри границы!" : "Коснись круга!";
            }

            try
            {
                if (!isSettingText)
                {
                    isSettingText = true;
                    __instance.text = translated;
                    isSettingText = false;
                    
                    int instanceId = __instance.GetInstanceID();
                    lock (translateLock)
                    {
                        currentTexts[instanceId] = translated;
                    }
                    
                    TranslationPlugin.logger.LogInfo($"FORCE TRANSLATED: '{value}' -> '{translated}'");
                }
            }
            catch (Exception e)
            {
                TranslationPlugin.logger.LogError($"Error in force translation: {e}");
                isSettingText = false;
            }
            return;
        }
        
        // 2. "I’m sorry..."
        if (value == "I’m sorry..." || value == "I’m sorry…")
        {
            lock (translateLock)
            {
                TranslationPlugin.logger.LogInfo($"=== I’M SORRY... DETECTED: '{value}' ===");
                
                int instanceId = __instance.GetInstanceID();
                
                if (!string.IsNullOrEmpty(value) && !IsSystemText(value))
                {
                    if (recentTextLog.Count > 20)
                    {
                        recentTextLog.RemoveAt(0);
                    }
                    recentTextLog.Add(value);
                }
                
                string apologyContext = GetApologyContext();
                TranslationPlugin.logger.LogInfo($"Apology context: {apologyContext}");
                
                if (apologyContext == "CONTEXT_APOLOGY_1")
                {
                    SetTranslatedText(__instance, "Мне жаль...", instanceId);
                    TranslationPlugin.logger.LogInfo($"I’m sorry... -> Мне жаль... (context 1: Why didn't you tell me?)");
                    return;
                }
                else if (apologyContext == "CONTEXT_APOLOGY_2")
                {
                    SetTranslatedText(__instance, "Прости...", instanceId);
                    TranslationPlugin.logger.LogInfo($"I’m sorry... -> Прости... (context 2: debt/after injury)");
                    return;
                }
                else if (apologyContext == "CONTEXT_APOLOGY_3")
                {
                    SetTranslatedText(__instance, "Мне очень жаль...", instanceId);
                    TranslationPlugin.logger.LogInfo($"I’m sorry... -> Мне очень жаль... (context 3: saving/apology expectation)");
                    return;
                }
                else
                {
                    SetTranslatedText(__instance, "Простите...", instanceId);
                    TranslationPlugin.logger.LogInfo($"I’m sorry... -> Простите... (default, no context)");
                    return;
                }
            }
        }
        
        // 3. "Right?!"
        if (value == "Right?!")
        {
            lock (translateLock)
            {
                TranslationPlugin.logger.LogInfo($"=== RIGHT?! DETECTED: '{value}' ===");
                
                int instanceId = __instance.GetInstanceID();
                
                if (!string.IsNullOrEmpty(value) && !IsSystemText(value))
                {
                    if (recentTextLog.Count > 20)
                    {
                        recentTextLog.RemoveAt(0);
                    }
                    recentTextLog.Add(value);
                }
                
                string rightExclamationContext = GetRightExclamationContext();
                TranslationPlugin.logger.LogInfo($"Right?! context: {rightExclamationContext}");
                
                if (rightExclamationContext == "CONTEXT_RIGHT_EXCLAMATION")
                {
                    SetTranslatedText(__instance, "А то?!", instanceId);
                    TranslationPlugin.logger.LogInfo($"Right?! -> А то?! (context)");
                    return;
                }
                else
                {
                    SetTranslatedText(__instance, "Точно?!", instanceId);
                    TranslationPlugin.logger.LogInfo($"Right?! -> Точно?! (default, no context)");
                    return;
                }
            }
        }
        
        // 4. "Happy now?"
        if (value == "Happy now?")
        {
            lock (translateLock)
            {
                TranslationPlugin.logger.LogInfo($"=== HAPPY NOW? DETECTED: '{value}' ===");
                
                int instanceId = __instance.GetInstanceID();
                
                if (!string.IsNullOrEmpty(value) && !IsSystemText(value))
                {
                    if (recentTextLog.Count > 20)
                    {
                        recentTextLog.RemoveAt(0);
                    }
                    recentTextLog.Add(value);
                }
                
                string happyNowContext = GetHappyNowContext();
                TranslationPlugin.logger.LogInfo($"Happy now? context: {happyNowContext}");
                
                if (happyNowContext == "CONTEXT_HAPPY_NOW_MORE")
                {
                    SetTranslatedText(__instance, "Может хватит?", instanceId);
                    TranslationPlugin.logger.LogInfo($"Happy now? -> Может хватит? (context: multiple 'More')");
                    return;
                }
                else
                {
                    SetTranslatedText(__instance, "Довольны теперь?", instanceId);
                    TranslationPlugin.logger.LogInfo($"Happy now? -> Довольны теперь? (default, no context)");
                    return;
                }
            }
        }

        // 5. "Why?"
        if (value == "Why?")
        {
            lock (translateLock)
            {
                TranslationPlugin.logger.LogInfo($"=== WHY? DETECTED: '{value}' ===");
                
                int instanceId = __instance.GetInstanceID();
                
                if (!string.IsNullOrEmpty(value) && !IsSystemText(value))
                {
                    if (recentTextLog.Count > 20)
                    {
                        recentTextLog.RemoveAt(0);
                    }
                    recentTextLog.Add(value);
                }
                
                string whyContext = GetWhyContext();
                TranslationPlugin.logger.LogInfo($"Why? context: {whyContext}");
                
                if (whyContext == "CONTEXT_WHY_GET_IN_MY_ARMS_and_BATHROOM")
                {
                    SetTranslatedText(__instance, "Зачем?", instanceId);
                    TranslationPlugin.logger.LogInfo($"Why? -> Зачем? (context: Get in my arms)");
                    return;
                }
                else if (whyContext == "CONTEXT_WHY_ACTION_REQUEST")
                {
                    SetTranslatedText(__instance, "Зачем?", instanceId);
                    TranslationPlugin.logger.LogInfo($"Why? -> Зачем? (context: action request)");
                    return;
                }
                else if (whyContext == "CONTEXT_WHY_DRINKING")
                {
                    SetTranslatedText(__instance, "Чего?", instanceId);
                    TranslationPlugin.logger.LogInfo($"Why? -> Чего? (context: drinking)");
                    return;
                }
                else if (whyContext == "CONTEXT_WHY_CHIEF")
                {
                    SetTranslatedText(__instance, "А?", instanceId);
                    TranslationPlugin.logger.LogInfo($"Why? -> А? (context: Chief)");
                    return;
                }
                else
                {
                    SetTranslatedText(__instance, "Почему?", instanceId);
                    TranslationPlugin.logger.LogInfo($"Why? -> Почему? (default, no context)");
                    return;
                }
            }
        }
        
        // 6. "Hold on..."
        if (value == "Hold on..." || value == "Hold on…")
        {
            lock (translateLock)
            {
                TranslationPlugin.logger.LogInfo($"=== HOLD ON... DETECTED: '{value}' ===");
                
                int instanceId = __instance.GetInstanceID();
                
                if (!string.IsNullOrEmpty(value) && !IsSystemText(value))
                {
                    if (recentTextLog.Count > 20)
                    {
                        recentTextLog.RemoveAt(0);
                    }
                    recentTextLog.Add(value);
                }
                
                string holdOnContext = GetHoldOnContext();
                TranslationPlugin.logger.LogInfo($"Hold on... context: {holdOnContext}");
                
                if (holdOnContext == "CONTEXT_HOLD_ON_HAIR_LOOSE")
                {
                    SetTranslatedText(__instance, "Замри...", instanceId);
                    TranslationPlugin.logger.LogInfo($"Hold on... -> Замри... (context: hair loose)");
                    return;
                }
                else
                {
                    SetTranslatedText(__instance, "Постой...", instanceId);
                    TranslationPlugin.logger.LogInfo($"Hold on... -> Постой... (default, no context)");
                    return;
                }
            }
        }

        // 7. "Arm"
        if (value == "Arm")
        {
            lock (translateLock)
            {
                TranslationPlugin.logger.LogInfo($"=== ARM DETECTED: '{value}' ===");
                
                int instanceId = __instance.GetInstanceID();
                
                if (!string.IsNullOrEmpty(value) && !IsSystemText(value))
                {
                    if (recentTextLog.Count > 20)
                    {
                        recentTextLog.RemoveAt(0);
                    }
                    recentTextLog.Add(value);
                }
                
                string armContext = GetArmContext();
                TranslationPlugin.logger.LogInfo($"Arm context: {armContext}");
                
                if (armContext == "CONTEXT_ARM_BODY_PARTS")
                {
                    SetTranslatedText(__instance, "Руки", instanceId);
                    TranslationPlugin.logger.LogInfo($"Arm -> Руки (context: body parts/medical)");
                    return;
                }
                else
                {
                    SetTranslatedText(__instance, "Рука", instanceId);
                    TranslationPlugin.logger.LogInfo($"Arm -> Рука (default, no context)");
                    return;
                }
            }
        }

        // 8. "Ugh, seriously..."
        if (value == "Ugh, seriously..." || value == "Ugh, seriously…")
        {
            lock (translateLock)
            {
                TranslationPlugin.logger.LogInfo($"=== UGH, SERIOUSLY... DETECTED: '{value}' ===");
                
                int instanceId = __instance.GetInstanceID();
                
                if (!string.IsNullOrEmpty(value) && !IsSystemText(value))
                {
                    if (recentTextLog.Count > 20)
                    {
                        recentTextLog.RemoveAt(0);
                    }
                    recentTextLog.Add(value);
                }
                
                string ughContext = GetUghSeriouslyContext();
                TranslationPlugin.logger.LogInfo($"Ugh, seriously... context: {ughContext}");
                
                if (ughContext == "CONTEXT_FIT_LOOSE")
                {
                    SetTranslatedText(__instance, "Ну, скотина...", instanceId);
                    TranslationPlugin.logger.LogInfo($"Ugh, seriously... -> Ну, скотина... (context: clothing used to fit loose)");
                    return;
                }
                else
                {
                    string defaultTranslation = TranslationPlugin.GetTranslation(value);
                    if (defaultTranslation == null)
                    {
                        defaultTranslation = "Ой, серьезно...";
                    }
                    SetTranslatedText(__instance, defaultTranslation, instanceId);
                    TranslationPlugin.logger.LogInfo($"Ugh, seriously... -> {defaultTranslation} (default, no context)");
                    return;
                }
            }
        }

        // 9. "Huh? Me?"
        if (value == "Huh? Me?")
        {
            lock (translateLock)
            {
                TranslationPlugin.logger.LogInfo($"=== HUH? ME? DETECTED: '{value}' ===");
                
                int instanceId = __instance.GetInstanceID();
                
                if (!string.IsNullOrEmpty(value) && !IsSystemText(value))
                {
                    if (recentTextLog.Count > 20)
                    {
                        recentTextLog.RemoveAt(0);
                    }
                    recentTextLog.Add(value);
                }
                
                string huhMeContext = GetHuhMeContext();
                TranslationPlugin.logger.LogInfo($"Huh? Me? context: {huhMeContext}");
                
                if (huhMeContext == "CONTEXT_SEE_YOU")
                {
                    SetTranslatedText(__instance, "А? Меня?", instanceId);
                    TranslationPlugin.logger.LogInfo($"Huh? Me? -> А? Меня? (context: 'whenever I see you')");
                    return;
                }
                else if (huhMeContext == "CONTEXT_TRY_NAIL_POLISH")
                {
                    SetTranslatedText(__instance, "А? Я?", instanceId);
                    TranslationPlugin.logger.LogInfo($"Huh? Me? -> А? Я? (context: nail polish/try)");
                    return;
                }
                else
                {
                    string defaultTranslation = TranslationPlugin.GetTranslation(value);
                    if (defaultTranslation == null)
                    {
                        defaultTranslation = "А? Со мной?";
                    }
                    SetTranslatedText(__instance, defaultTranslation, instanceId);
                    TranslationPlugin.logger.LogInfo($"Huh? Me? -> {defaultTranslation} (default, no context)");
                    return;
                }
            }
        }

        // 10. Colors
        if (value == "White" || value == "Black" || value == "Green" || 
            value == "Blue" || value == "Yellow" || value == "Pink" || value == "Red")
        {
            HandleColorText(__instance, value);
            return;
        }

        // 11. "Huh? Why?"
        if (value == "Huh? Why?")
        {
            lock (translateLock)
            {
                TranslationPlugin.logger.LogInfo($"=== HUH? WHY? DETECTED: '{value}' ===");
                
                int instanceId = __instance.GetInstanceID();
                
                if (!string.IsNullOrEmpty(value) && !IsSystemText(value))
                {
                    if (recentTextLog.Count > 20)
                    {
                        recentTextLog.RemoveAt(0);
                    }
                    recentTextLog.Add(value);
                }
                
                string huhWhyContext = GetHuhWhyContext();
                TranslationPlugin.logger.LogInfo($"Huh? Why? context: {huhWhyContext}");
                
                if (huhWhyContext == "CONTEXT_HUH_WHY_SMOOTH_GUY")
                {
                    SetTranslatedText(__instance, "А? Почему?", instanceId);
                    TranslationPlugin.logger.LogInfo($"Huh? Why? -> А? Почему? (context: smooth guy)");
                    return;
                }
                else
                {
                    SetTranslatedText(__instance, "А? А что?", instanceId);
                    TranslationPlugin.logger.LogInfo($"Huh? Why? -> А? А что? (default, no context)");
                    return;
                }
            }
        }

        // 12. "Ah~" (для ам - еды)
        if (value == "Ah~")
        {
            lock (translateLock)
            {
                TranslationPlugin.logger.LogInfo($"=== AH~ DETECTED: '{value}' ===");
                
                int instanceId = __instance.GetInstanceID();
                
                if (!string.IsNullOrEmpty(value) && !IsSystemText(value))
                {
                    if (recentTextLog.Count > 20)
                    {
                        recentTextLog.RemoveAt(0);
                    }
                    recentTextLog.Add(value);
                }
                
                string ahContext = GetAhContext();
                TranslationPlugin.logger.LogInfo($"Ah~ context: {ahContext}");
                
                if (ahContext == "CONTEXT_AH_FOOD")
                {
                    SetTranslatedText(__instance, "“Аа~”", instanceId);
                    TranslationPlugin.logger.LogInfo($"Ah~ -> Аа~ (context: medical)");
                    return;
                }
                else
                {
                    SetTranslatedText(__instance, "Ах~", instanceId);
                    TranslationPlugin.logger.LogInfo($"Ah~ -> Ах~ (default, no context)");
                    return;
                }
            }
        }

        // 13. "Talk"
        if (value == "Talk" || value == "Talk.")
        {
            lock (translateLock)
            {
                TranslationPlugin.logger.LogInfo($"=== TALK DETECTED: '{value}' ===");
                
                int instanceId = __instance.GetInstanceID();
                
                if (!string.IsNullOrEmpty(value) && !IsSystemText(value))
                {
                    if (recentTextLog.Count > 20)
                    {
                        recentTextLog.RemoveAt(0);
                    }
                    recentTextLog.Add(value);
                }
                
                string talkContext = GetTalkContext();
                TranslationPlugin.logger.LogInfo($"Talk context: {talkContext}");
                
                if (talkContext == "CONTEXT_TALK_ACTION_CHOICE")
                {
                    SetTranslatedText(__instance, "Поговорить", instanceId);
                    TranslationPlugin.logger.LogInfo($"Talk -> Поговорить (context: action choice with 'Run Away')");
                    return;
                }
                else
                {
                    SetTranslatedText(__instance, "Сказать", instanceId);
                    TranslationPlugin.logger.LogInfo($"Talk -> Сказать (default, no context)");
                    return;
                }
            }
        }

        // 14. "What is it?"
        if (value == "What is it?")
        {
            lock (translateLock)
            {
                TranslationPlugin.logger.LogInfo($"=== WHAT IS IT? DETECTED: '{value}' ===");
                
                int instanceId = __instance.GetInstanceID();
                
                if (!string.IsNullOrEmpty(value) && !IsSystemText(value))
                {
                    if (recentTextLog.Count > 20)
                    {
                        recentTextLog.RemoveAt(0);
                    }
                    recentTextLog.Add(value);
                }
                
                string context = GetWhatIsItContext();
                TranslationPlugin.logger.LogInfo($"What is it? context: {context}");
                
                if (context == "CONTEXT_WHAT_IS_IT_SCARED")
                {
                    SetTranslatedText(__instance, "Что такое?", instanceId);
                    TranslationPlugin.logger.LogInfo($"What is it? -> Что такое? (context: after being scared)");
                    return;
                }
                else if (context == "CONTEXT_WHAT_IS_IT_SECRET_MOVE")
                {
                    SetTranslatedText(__instance, "Какой же?", instanceId);
                    TranslationPlugin.logger.LogInfo($"What is it? -> Какой же? (context: secret move)");
                    return;
                }
                else
                {
                    SetTranslatedText(__instance, "Что это?", instanceId);
                    TranslationPlugin.logger.LogInfo($"What is it? -> Что это? (default, no context)");
                    return;
                }
            }
        }

        // 15. "You sure?"
        if (value == "You sure?" || value == "You sure…" || value == "You sure...")
        {
            lock (translateLock)
            {
                TranslationPlugin.logger.LogInfo($"=== YOU SURE? DETECTED: '{value}' ===");
                
                int instanceId = __instance.GetInstanceID();
                
                if (!string.IsNullOrEmpty(value) && !IsSystemText(value))
                {
                    if (recentTextLog.Count > 20)
                    {
                        recentTextLog.RemoveAt(0);
                    }
                    recentTextLog.Add(value);
                }
                
                string context = GetYouSureContext();
                TranslationPlugin.logger.LogInfo($"You sure? context: {context}");
                
                if (context == "CONTEXT_YOU_SURE_FEMALE")
                {
                    SetTranslatedText(__instance, "Уверена?", instanceId);
                    TranslationPlugin.logger.LogInfo($"You sure? -> Уверена? (female context)");
                    return;
                }
                else
                {
                    SetTranslatedText(__instance, "Уверен?", instanceId);
                    TranslationPlugin.logger.LogInfo($"You sure? -> Уверен? (default, male context)");
                    return;
                }
            }
        }

        // 16. "Ah! Sorry!"
        if (value == "Ah! Sorry!" || value == "Ah! Sorry…" || value == "Ah! Sorry...")
        {
            lock (translateLock)
            {
                TranslationPlugin.logger.LogInfo($"=== AH! SORRY! DETECTED: '{value}' ===");
                
                int instanceId = __instance.GetInstanceID();
                
                if (!string.IsNullOrEmpty(value) && !IsSystemText(value))
                {
                    if (recentTextLog.Count > 20)
                    {
                        recentTextLog.RemoveAt(0);
                    }
                    recentTextLog.Add(value);
                }
                
                string context = GetAhSorryContext();
                TranslationPlugin.logger.LogInfo($"Ah! Sorry! context: {context}");
                
                if (context == "CONTEXT_AH_SORRY_LET_GO")
                {
                    SetTranslatedText(__instance, "Ой! Прости!", instanceId);
                    TranslationPlugin.logger.LogInfo($"Ah! Sorry! -> Ой! Прости! (context: let go)");
                    return;
                }
                else
                {
                    SetTranslatedText(__instance, "Ай! Простите!", instanceId);
                    TranslationPlugin.logger.LogInfo($"Ah! Sorry! -> Ай! Простите! (default, no context)");
                    return;
                }
            }
        }

        // 17. "White?"
        if (value == "White?")
        {
            lock (translateLock)
            {
                TranslationPlugin.logger.LogInfo($"=== WHITE? DETECTED: '{value}' ===");
                
                int instanceId = __instance.GetInstanceID();
                
                if (!string.IsNullOrEmpty(value) && !IsSystemText(value))
                {
                    if (recentTextLog.Count > 20)
                    {
                        recentTextLog.RemoveAt(0);
                    }
                    recentTextLog.Add(value);
                }
                
                string context = GetWhiteContext();
                TranslationPlugin.logger.LogInfo($"White? context: {context}");
                
                if (context == "CONTEXT_WHITE_SINGULAR")
                {
                    SetTranslatedText(__instance, "Белый?", instanceId);
                    TranslationPlugin.logger.LogInfo($"White? -> Белый? (context: wearing white)");
                    return;
                }
                else
                {
                    SetTranslatedText(__instance, "Белые?", instanceId);
                    TranslationPlugin.logger.LogInfo($"White? -> Белые? (default, plural)");
                    return;
                }
            }
        }

        // 18. "Sounds good."
        if (value == "Sounds good.")
        {
            lock (translateLock)
            {
                TranslationPlugin.logger.LogInfo($"=== SOUNDS GOOD DETECTED: '{value}' ===");
                
                int instanceId = __instance.GetInstanceID();
                
                if (!string.IsNullOrEmpty(value) && !IsSystemText(value))
                {
                    if (recentTextLog.Count > 20)
                    {
                        recentTextLog.RemoveAt(0);
                    }
                    recentTextLog.Add(value);
                }
                
                string context = GetSoundsGoodContext();
                TranslationPlugin.logger.LogInfo($"Sounds good. context: {context}");
                
                if (context == "CONTEXT_SOUNDS_GOOD_GROUP_ENTRY")
                {
                    SetTranslatedText(__instance, "Безусловно.", instanceId);
                    TranslationPlugin.logger.LogInfo($"Sounds good. -> Безусловно. (context: group entry)");
                    return;
                }
                else
                {
                    SetTranslatedText(__instance, "Без проблем.", instanceId);
                    TranslationPlugin.logger.LogInfo($"Sounds good. -> Без проблем. (default)");
                    return;
                }
            }
        }

        // 19. "Take a look."
        if (value == "Take a look.")
        {
            lock (translateLock)
            {
                TranslationPlugin.logger.LogInfo($"=== TAKE A LOOK DETECTED: '{value}' ===");
                
                int instanceId = __instance.GetInstanceID();
                
                if (!string.IsNullOrEmpty(value) && !IsSystemText(value))
                {
                    if (recentTextLog.Count > 20)
                    {
                        recentTextLog.RemoveAt(0);
                    }
                    recentTextLog.Add(value);
                }
                
                string context = GetTakeALookContext();
                TranslationPlugin.logger.LogInfo($"Take a look. context: {context}");
                
                if (context == "CONTEXT_TAKE_A_LOOK_PHOTO")
                {
                    SetTranslatedText(__instance, "Посмотри.", instanceId);
                    TranslationPlugin.logger.LogInfo($"Take a look. -> Посмотри. (context: photo result)");
                    return;
                }
                else
                {
                    SetTranslatedText(__instance, "Дай гляну.", instanceId);
                    TranslationPlugin.logger.LogInfo($"Take a look. -> Дай гляну. (default)");
                    return;
                }
            }
        }

        // 20. "Coming!"
        if (value == "Coming!")
        {
            lock (translateLock)
            {
                TranslationPlugin.logger.LogInfo($"=== COMING! DETECTED: '{value}' ===");
                
                int instanceId = __instance.GetInstanceID();
                
                if (!string.IsNullOrEmpty(value) && !IsSystemText(value))
                {
                    if (recentTextLog.Count > 20)
                    {
                        recentTextLog.RemoveAt(0);
                    }
                    recentTextLog.Add(value);
                }
                
                string context = GetComingContext();
                TranslationPlugin.logger.LogInfo($"Coming! context: {context}");
                
                if (context == "CONTEXT_COMING_SOCKS")
                {
                    SetTranslatedText(__instance, "Сейчас!", instanceId);
                    TranslationPlugin.logger.LogInfo($"Coming! -> Сейчас! (context: socks request)");
                    return;
                }
                else
                {
                    SetTranslatedText(__instance, "Иду!", instanceId);
                    TranslationPlugin.logger.LogInfo($"Coming! -> Иду! (default)");
                    return;
                }
            }
        }

        // 21. "Hello..."
        if (value == "Hello...")
        {
            lock (translateLock)
            {
                TranslationPlugin.logger.LogInfo($"=== HELLO... DETECTED: '{value}' ===");
                
                int instanceId = __instance.GetInstanceID();
                
                if (!string.IsNullOrEmpty(value) && !IsSystemText(value))
                {
                    if (recentTextLog.Count > 20)
                    {
                        recentTextLog.RemoveAt(0);
                    }
                    recentTextLog.Add(value);
                }
                
                string context = GetHelloContext();
                TranslationPlugin.logger.LogInfo($"Hello... context: {context}");
                
                if (context == "CONTEXT_HELLO_WUSEON")
                {
                    SetTranslatedText(__instance, "Привет...", instanceId);
                    TranslationPlugin.logger.LogInfo($"Hello... -> Привет... (context: Wuseon greeting)");
                    return;
                }
                else
                {
                    SetTranslatedText(__instance, "Алло...", instanceId);
                    TranslationPlugin.logger.LogInfo($"Hello... -> Алло... (default)");
                    return;
                }
            }
        }

        // 22. Остальной код...
        lock (translateLock)
        {
            try
            {
                 CheckAndResetContext();

                int instanceId = __instance.GetInstanceID();
                
                if (currentTexts.TryGetValue(instanceId, out string cachedText))
                {
                    if (cachedText == value) return;
                    
                    if (__instance.text != cachedText)
                    {
                        currentTexts.Remove(instanceId);
                    }
                }
                
                CheckAndResetContext();
                
                if ((System.DateTime.Now - lastMeDetection).TotalSeconds > 5.0 && 
                    pendingMeTranslations.Count > 0)
                {
                    TranslationPlugin.logger.LogInfo($"5s timeout reached for pending Me?, processing with default");
                    ProcessPendingMeWithDefault();
                }
                
                if (!string.IsNullOrEmpty(value) && !IsSystemText(value))
                {
                    if (recentTextLog.Count > 20)
                    {
                        recentTextLog.RemoveAt(0);
                    }
                    recentTextLog.Add(value);
                }
                
                if (value.Contains("Feeds Her") || value == "Me?" || value == "Right?" || value == "White" || value == "Black" || value == "Happy now?")
                {
                    TranslationPlugin.logger.LogInfo("=== ТЕКСТ ОБНАРУЖЕН ===");
                    TranslationPlugin.logger.LogInfo($"Текст: '{value}'");
                    TranslationPlugin.logger.LogInfo($"Текущий бандл: {BundleTracker.CurrentBundle ?? "NULL"}");
                    TranslationPlugin.logger.LogInfo($"Имя сцены: {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}");
                    TranslationPlugin.logger.LogInfo($"Объект: {__instance.gameObject.name}");
                    TranslationPlugin.logger.LogInfo($"Текущий контекст: {colorContext}");
                    TranslationPlugin.logger.LogInfo($"Контекст Are you okay?: {areYouOkayContext}");
                    TranslationPlugin.logger.LogInfo($"Контекст использован: {wasContextUsed}");
                    TranslationPlugin.logger.LogInfo($"Цветов в серии: {processedColorsInSeries}");
                    
                    Transform parent = __instance.transform.parent;
                    while (parent != null)
                    {
                        TranslationPlugin.logger.LogInfo($"Родитель: {parent.name}");
                        parent = parent.parent;
                    }
                    TranslationPlugin.logger.LogInfo("======================");
                }

                if (IsYuNaSceneMarker(value))
                {
                    TranslationPlugin.logger.LogInfo($"YUNA SCENE MARKER DETECTED: '{value}'");
                    ProcessPendingFeedsHer("YUNA");
                }

                if (value == "Doctor" || value == "Доктор")
                {
                    TranslationPlugin.logger.LogInfo($"DOCTOR MARKER DETECTED - checking if we should change translation");

                    bool hasPresenceQuestionInHistory = false;
                    foreach (string log in recentTextLog)
                    {
                        if (log.Contains("What Are You Doing Here") || 
                            log.Contains("What are you doing here") ||
                            log.Contains("What do you want here"))
                        {
                            hasPresenceQuestionInHistory = true;
                            TranslationPlugin.logger.LogInfo($"Found presence question in history: {log}");
                            break;
                        }
                    }
                    
                    if (hasPresenceQuestionInHistory && pendingMeTranslations.Count > 0)
                    {
                        TranslationPlugin.logger.LogInfo($"Changing {pendingMeTranslations.Count} pending 'Я?' to 'Обо мне?' due to doctor marker");
                        
                        var pendingIds = pendingMeTranslations.Keys.ToList();
                        
                        foreach (var pendingId in pendingIds)
                        {
                            if (pendingMeTranslations.ContainsKey(pendingId))
                            {
                                var pendingText = pendingMeTranslations[pendingId];
                                if (pendingText != null && pendingText.gameObject != null)
                                {
                                    pendingText.text = "Обо мне?";
                                    currentTexts[pendingId] = "Обо мне?";
                                    TranslationPlugin.logger.LogInfo($"Changed pending Me? (ID: {pendingId}) to 'Обо мне?'");
                                    
                                    pendingMeTranslations.Remove(pendingId);
                                }
                            }
                        }
                    }
                }

                if (value.Contains("stethoscope") || value.Contains("Gives a tangerine") || 
                    value.Contains("Do you like mandarins") || value.Contains("See? I told you") ||
                    value.Contains("Once you’re feeling better") || value.Contains("Когда тебе станет лучше"))
                {
                    if (pendingMeTranslations.Count > 0 || pendingRightTranslations.Count > 0 || 
                        pendingFeedsHerTranslations.Count > 0 || pendingColorTranslations.Count > 0)
                    {
                        TranslationPlugin.logger.LogInfo($"Scene marker detected, clearing pending translations");
                        ClearPendingTranslations();
                    }
                    ResetAreYouOkayContext();
                }

                if ((System.DateTime.Now - lastMeDetection).TotalSeconds > 30 && 
                    (pendingRightTranslations.Count > 0))
                {
                    TranslationPlugin.logger.LogInfo($"30s timeout reached, clearing pending Right? translations");
                    pendingRightTranslations.Clear();
                }

                if (value == "Feeds Her")
                {
                    TranslationPlugin.logger.LogInfo($"Feeds Her detected (ID: {instanceId})");
                    
                    string detectedScene = DetectSceneFromHistory();
                    TranslationPlugin.logger.LogInfo($"Detected scene: {detectedScene}");
                    
                    if (detectedScene == "GYURI")
                    {
                        SetTranslatedText(__instance, "Накормить ее", instanceId);
                        TranslationPlugin.logger.LogInfo("Feeds Her -> Накормить ее (GyuRi scene)");
                        return;
                    }
                    else if (detectedScene == "YUNA")
                    {
                        SetTranslatedText(__instance, "Напоить ее", instanceId);
                        TranslationPlugin.logger.LogInfo("Feeds Her -> Напоить ее (YuNa scene)");
                        return;
                    }
                    else
                    {
                        pendingFeedsHerTranslations[instanceId] = __instance;
                        TranslationPlugin.logger.LogInfo($"Feeds Her -> PENDING (unknown scene, saved ID: {instanceId})");
                        
                        currentTexts[instanceId] = value;
                        return;
                    }
                }

                if (value == "Got it." || value == "Got it!")
                {
                    lock (translateLock)
                    {
                        TranslationPlugin.logger.LogInfo($"=== GOT IT DETECTED: '{value}' ===");
                        
                        if (!string.IsNullOrEmpty(value) && !IsSystemText(value))
                        {
                            if (recentTextLog.Count > 20)
                            {
                                recentTextLog.RemoveAt(0);
                            }
                            recentTextLog.Add(value);
                        }
                        
                        bool isFemaleSpeakerContext = false;
                        bool isCaughtContext = false;
                        bool isAgreementContext = false;
                        
                        int checkCount = Math.Min(3, recentTextLog.Count);
                        for (int i = Math.Max(0, recentTextLog.Count - checkCount); i < recentTextLog.Count; i++)
                        {
                            string recentText = recentTextLog[i];
                            if (string.IsNullOrEmpty(recentText)) continue;
                            
                            TranslationPlugin.logger.LogInfo($"  Recent [{i}]: '{recentText}'");
                            
                            if (recentText == "Watching you work at the hospital… it always looks so exhausting." ||
                                recentText == "Смотря, как ты работаешь в больнице… всегда кажется, что это так изматывает." ||
                                recentText == "Make sure you really take it easy today!" ||
                                recentText == "Обязательно как следует отдохни сегодня!" ||
                                recentText == "You too, Yuman—don’t overdo it, okay?" ||
                                recentText == "Ты тоже, Юман — не переусердствуй, ладно?")
                            {
                                isFemaleSpeakerContext = true;
                                TranslationPlugin.logger.LogInfo($"  -> Yuman GotIt context detected (exact phrase match)");
                                break;
                            }
                            
                            if (recentText == "But don’t you have to hurry back and get the stuff?" ||
                                recentText == "Но разве тебе не нужно спешить обратно за вещами?" ||
                                recentText == "Nah, it’s just supplies for the boarding house—it’s fine!" ||
                                recentText == "Не, это просто припасы для пансиона — ничего страшного!" ||
                                recentText == "Oh~ you didn’t know? I run a boarding house." ||
                                recentText == "О~ ты не знал? Я управляю пансионом." ||
                                recentText == "Ah, so that person I saw with you before... they’re just a tenant?" ||
                                recentText == "А, значит тот человек, которого я видел с тобой раньше... он просто жилец?")
                            {
                                isCaughtContext = true;
                                TranslationPlugin.logger.LogInfo($"  -> Caught context detected (exact phrase match)");
                                break;
                            }
                            
                            if (recentText == "Alright, deal?" ||
                                recentText == "Ладно, договорились?")
                            {
                                isAgreementContext = true;
                                TranslationPlugin.logger.LogInfo($"  -> Agreement context detected (school beauty)");
                                break;
                            }
                        }
                        
                        string translation;
                        if (value == "Got it.")
                        {
                            if (isFemaleSpeakerContext)
                            {
                                translation = "Принял.";
                                TranslationPlugin.logger.LogInfo($"Got it. -> Принял. (Yuman GotIt context)");
                            }
                            else if (isAgreementContext)
                            {
                                translation = "Договорились.";
                                TranslationPlugin.logger.LogInfo($"Got it. -> Договорились. (agreement context)");
                            }
                            else
                            {
                                translation = "Поняла.";
                                TranslationPlugin.logger.LogInfo($"Got it. -> Поняла. (default)");
                            }
                        }
                        else
                        {
                            if (isCaughtContext)
                            {
                                translation = "Отлично!";
                                TranslationPlugin.logger.LogInfo($"Got it! -> Отлично! (caught exact context)");
                            }
                            else
                            {
                                translation = "Попалась!";
                                TranslationPlugin.logger.LogInfo($"Got it! -> Попалась! (default)");
                            }
                        }
                        
                        SetTranslatedText(__instance, translation, instanceId);
                        return;
                    }
                }

                if (value == "You sure about that?")
                {
                    TranslationPlugin.logger.LogInfo($"=== YOU SURE ABOUT THAT? DETECTED ===");
                    
                    bool isFemaleSpeakerContext = false;
                    
                    int checkCount = Math.Min(3, recentTextLog.Count);
                    for (int i = Math.Max(0, recentTextLog.Count - checkCount); i < recentTextLog.Count; i++)
                    {
                        string recentText = recentTextLog[i];
                        if (string.IsNullOrEmpty(recentText)) continue;
                        
                        TranslationPlugin.logger.LogInfo($"  Recent [{i}]: '{recentText}'");
                        
                        if (recentText == "I definitely saw it—" ||
                            recentText == "Я точно видела—" ||
                            recentText == "trust me." ||
                            recentText == "поверь мне." ||
                            recentText == "Going with card 3, then?" ||
                            recentText == "Значит, выбираем карту 3?")
                        {
                            isFemaleSpeakerContext = true;
                            TranslationPlugin.logger.LogInfo($"  -> Female speaker context detected (exact phrase match)");
                            break;
                        }
                    }
                    
                    string translation;
                    if (isFemaleSpeakerContext)
                    {
                        translation = "Уверена?";
                        TranslationPlugin.logger.LogInfo($"You sure about that? -> Уверена? (female speaker context)");
                    }
                    else
                    {
                        translation = "Уверен?";
                        TranslationPlugin.logger.LogInfo($"You sure about that? -> Уверен? (default)");
                    }
                    
                    SetTranslatedText(__instance, translation, instanceId);
                    return;
                }

                if (value == "Me?")
                {
                    lastMeDetection = System.DateTime.Now;
                    
                    TranslationPlugin.logger.LogInfo($"=== ME? DETECTED ===");
                    TranslationPlugin.logger.LogInfo($"Last dialogue (original): '{lastLongDialogueOriginal}'");
                    TranslationPlugin.logger.LogInfo($"Last dialogue (translated): '{lastLongDialogue}'");
                    
                    string context = GetMeContext();
                    TranslationPlugin.logger.LogInfo($"Me? context: {context}");
                    
                    bool hasStethoscopeInHistory = false;
                    bool hasPresenceQuestionInHistory = false;
                    
                    int checkCount = Math.Min(3, recentTextLog.Count);
                    for (int i = recentTextLog.Count - checkCount; i < recentTextLog.Count; i++)
                    {
                        if (i < 0) continue;
                        
                        string log = recentTextLog[i];
                        
                        if (log.Contains("stethoscope") || log.Contains("стетоскоп"))
                        {
                            hasStethoscopeInHistory = true;
                            TranslationPlugin.logger.LogInfo($"Found stethoscope in recent history [{i}]: {log}");
                        }
                        
                        if (log.Contains("What Are You Doing Here") || 
                            log.Contains("What are you doing here") ||
                            log.Contains("What do you want here"))
                        {
                            hasPresenceQuestionInHistory = true;
                            TranslationPlugin.logger.LogInfo($"Found presence question in recent history [{i}]: {log}");
                        }
                    }
                    
                    if (context == "STETHOSCOPE" || context == "MOVING_IN" || 
                        context == "SPORTS" || context == "TOGETHER" || context == "INVITATION" ||
                        hasStethoscopeInHistory)
                    {
                        SetTranslatedText(__instance, "Я?", instanceId);
                        TranslationPlugin.logger.LogInfo($"Me? -> Я? ({context} context, stethoscope in recent: {hasStethoscopeInHistory})");
                        return;
                    }
                    else if (context == "ABOUT_DOCTOR" || context == "PRESENCE_QUESTION")
                    {
                        SetTranslatedText(__instance, "Обо мне?", instanceId);
                        TranslationPlugin.logger.LogInfo($"Me? -> Обо мне? ({context} context)");
                        return;
                    }
                    
                    if (hasPresenceQuestionInHistory)
                    {
                        SetTranslatedText(__instance, "Обо мне?", instanceId);
                        TranslationPlugin.logger.LogInfo("Me? -> Обо мне? (presence question in recent history)");
                        return;
                    }
                    
                    if (context == "DEFAULT")
                    {
                        SetTranslatedText(__instance, "Я?", instanceId);
                        TranslationPlugin.logger.LogInfo("Me? -> Я? (default context with enough history)");
                        return;
                    }
                    else if (context == "UNKNOWN" || context == "DEFAULT")
                    {
                        SetTranslatedText(__instance, "Я?", instanceId);
                        TranslationPlugin.logger.LogInfo($"Me? -> Я? ({context} context, default translation)");
                        return;
                    }
                    else
                    {
                        SetTranslatedText(__instance, "Я?", instanceId);
                        TranslationPlugin.logger.LogInfo("Me? -> Я? (fallback)");
                        return;
                    }
                }

                if (value == "Here you go.")
                {
                    TranslationPlugin.logger.LogInfo($"=== HERE YOU GO DETECTED ===");
                    
                    bool isInformalContext = false;
                    
                    int checkCount = Math.Min(3, recentTextLog.Count);
                    for (int i = Math.Max(0, recentTextLog.Count - checkCount); i < recentTextLog.Count; i++)
                    {
                        string recentText = recentTextLog[i];
                        if (string.IsNullOrEmpty(recentText)) continue;
                        
                        TranslationPlugin.logger.LogInfo($"  Recent [{i}]: '{recentText}'");
                        
                        if (recentText == "Thank you." ||
                            recentText == "Спасибо." ||
                            recentText == "Let’s give the gentleman the honor." ||
                            recentText == "Прошу джентльмена принять честь!" ||
                            recentText == "Yes, just a moment please." ||
                            recentText == "Да, момент, пожалуйста.")
                        {
                            isInformalContext = true;
                            TranslationPlugin.logger.LogInfo($"  -> Informal context detected (exact phrase match)");
                            break;
                        }
                    }
                    
                    string translation;
                    if (isInformalContext)
                    {
                        translation = "Держи.";
                        TranslationPlugin.logger.LogInfo($"Here you go. -> Держи. (informal context)");
                    }
                    else
                    {
                        translation = "Держите.";
                        TranslationPlugin.logger.LogInfo($"Here you go. -> Держите. (default/formal)");
                    }
                    
                    SetTranslatedText(__instance, translation, instanceId);
                    return;
                }

                if (value == "Right?")
                {
                    TranslationPlugin.logger.LogInfo($"=== RIGHT? DETECTED ===");
                    TranslationPlugin.logger.LogInfo($"Last dialogue (original): '{lastLongDialogueOriginal}'");
                    TranslationPlugin.logger.LogInfo($"Last dialogue (translated): '{lastLongDialogue}'");
                    
                    string context = GetRightContext();
                    TranslationPlugin.logger.LogInfo($"Right? context: {context}");
                    
                    if (context == "CONFIRMATION")
                    {
                        SetTranslatedText(__instance, "Точно?", instanceId);
                        TranslationPlugin.logger.LogInfo("Right? -> Точно? (confirmation context)");
                        return;
                    }
                    else if (context == "AGREEMENT")
                    {
                        SetTranslatedText(__instance, "Да?", instanceId);
                        TranslationPlugin.logger.LogInfo("Right? -> Да? (agreement context)");
                        return;
                    }
                    else if (context == "NORMALITY")
                    {
                        SetTranslatedText(__instance, "Верно?", instanceId);
                        TranslationPlugin.logger.LogInfo("Right? -> Верно? (normality context)");
                        return;
                    }
                    else
                    {
                        pendingRightTranslations[instanceId] = __instance;
                        TranslationPlugin.logger.LogInfo($"Right? -> PENDING (unknown context, ID: {instanceId})");
                        
                        currentTexts[instanceId] = value;
                        return;
                    }
                }

                if (value == "Are you okay?")
                {
                    TranslationPlugin.logger.LogInfo($"=== ARE YOU OKAY? DETECTED ===");
                    TranslationPlugin.logger.LogInfo($"Last dialogue (original): '{lastLongDialogueOriginal}'");
                    
                    string context = GetAreYouOkayContext();
                    TranslationPlugin.logger.LogInfo($"Are you okay? context: {context}");
                    
                    if (context == "MASSAGE")
                    {
                        SetTranslatedText(__instance, "Все хорошо?", instanceId);
                        TranslationPlugin.logger.LogInfo("Are you okay? -> Все хорошо? (massage context)");
                    }
                    else
                    {
                        SetTranslatedText(__instance, "С тобой все в порядке?", instanceId);
                        TranslationPlugin.logger.LogInfo("Are you okay? -> С тобой все в порядке? (default context)");
                    }
                    
                    ResetAreYouOkayContext();
                    
                    lastLongDialogue = "";
                    lastLongDialogueOriginal = "";
                    
                    return;
                }

                Action<string> setText = (newText) => 
                {
                    try
                    {
                        if (__instance == null || __instance.gameObject == null) return;
                        if (isSettingText) return;
                        
                        isSettingText = true;
                        __instance.text = newText;
                        currentTexts[instanceId] = newText;
                        isSettingText = false;
                    }
                    catch (Exception e)
                    {
                        TranslationPlugin.logger.LogError($"Error in TMP setText: {e}");
                        isSettingText = false;
                    }
                };
                
                ProcessText(__instance.gameObject, value, instanceId, setText);
            }
            catch (Exception e)
            {
                TranslationPlugin.logger.LogError($"Error in TranslateTMPText: {e}");
            }
        }
    }

    private static void HandleColorText(TMP_Text component, string colorValue)
    {
        lock (translateLock)
        {
            int instanceId = component.GetInstanceID();
            
            TranslationPlugin.logger.LogInfo($"=== COLOR DETECTED: {colorValue} ===");
            TranslationPlugin.logger.LogInfo($"Current color context: {colorContext}");

            if (!string.IsNullOrEmpty(colorContext) && wasContextUsed)
            {
                double timeSinceContextUsed = (System.DateTime.Now - lastContextUsedTime).TotalSeconds;
                if (timeSinceContextUsed > 10.0f)
                {
                    TranslationPlugin.logger.LogInfo($"[STALE CONTEXT] Context '{colorContext}' active for {timeSinceContextUsed:F2}s, resetting");
                    ResetColorContext();
                }
            }
            
            if (colorContext == "socks" && processedColorsInSeries >= 3)
            {
                TranslationPlugin.logger.LogInfo($"[CONTEXT ERROR] socks context still active after processing {processedColorsInSeries} colors, resetting");
                ResetColorContext();
            }
            
            if (string.IsNullOrEmpty(colorContext))
            {
                string translation = GetColorTranslation(colorValue, "");
                TranslationPlugin.logger.LogInfo($"{colorValue} -> {translation} (default, no context)");
                SetTranslatedText(component, translation, instanceId);
                return;
            }
            
            switch (colorContext)
            {
                case "swimsuit":
                    ProcessSwimsuitColor(component, colorValue, instanceId);
                    break;
                case "undies":
                    ProcessUndiesColor(component, colorValue, instanceId);
                    break;
                case "socks":
                    ProcessSocksColor(component, colorValue, instanceId);
                    break;
                default:
                    SetTranslatedText(component, colorValue, instanceId);
                    break;
            }
        }
    }

    private static void ProcessSwimsuitColor(TMP_Text component, string colorValue, int instanceId)
    {
        string translation = GetColorTranslation(colorValue, "swimsuit");
        TranslationPlugin.logger.LogInfo($"{colorValue} -> {translation} (swimsuit context)");
        
        SetTranslatedText(component, translation, instanceId);
        
        processedColorsInSeries++;
        TranslationPlugin.logger.LogInfo($"[SWIMSUIT] Processed {processedColorsInSeries}/4 colors");
        
        if (processedColorsInSeries >= 4)
        {
            TranslationPlugin.logger.LogInfo($"[CONTEXT COMPLETE] All {processedColorsInSeries} swimsuit colors processed");
            ResetColorContext();
        }
    }

    private static void ProcessUndiesColor(TMP_Text component, string colorValue, int instanceId)
    {
        string translation = GetColorTranslation(colorValue, "undies");
        TranslationPlugin.logger.LogInfo($"{colorValue} -> {translation} (undies context)");
        
        SetTranslatedText(component, translation, instanceId);
        
        processedColorsInSeries++;
        TranslationPlugin.logger.LogInfo($"[UNDIES] Processed {processedColorsInSeries}/3 colors");
        
        if (processedColorsInSeries >= 3)
        {
            TranslationPlugin.logger.LogInfo($"[CONTEXT COMPLETE] All {processedColorsInSeries} undies colors processed");
            ResetColorContext();
        }
    }



    private static void SetTranslatedText(TMP_Text component, string text, int instanceId)
    {
        lock (translateLock)
        {
            try
            {
                if (isSettingText)
                    return;
                    
                isSettingText = true;
                
                string originalText = component.text;
                
                if (originalText == text)
                {
                    isSettingText = false;
                    return;
                }
                
                component.text = text;
                
                currentTexts[instanceId] = text;
                
                TranslationPlugin.logger.LogInfo($"[SetTranslatedText] '{originalText}' -> '{text}'");
                
                isSettingText = false;
            }
            catch (Exception e)
            {
                TranslationPlugin.logger.LogError($"Error in SetTranslatedText: {e}");
                isSettingText = false;
            }
        }
    }

    private static void UpdateColorContext(string translatedText)
    {
        lock (translateLock)
        {
            processedColorsInSeries = 0;
            
            if (translatedText.Contains("купальник") || translatedText.Contains("swimsuit") ||
                translatedText.Contains("What color was my swimsuit") ||
                translatedText.Contains("Тогда ты помнишь, как мы поехали на пляж"))
            {
                colorContext = "swimsuit";
                TranslationPlugin.logger.LogInfo($"[COLOR CONTEXT SET] Set to: swimsuit (from: '{translatedText}')");
                ProcessPendingColorsForContext();
            }
            else if (translatedText.Contains("трусики") || translatedText.Contains("они?") || 
                    translatedText.Contains("What color was it?!") ||
                    translatedText.Contains("Твои трусики") || translatedText.Contains("Your undies") ||
                    translatedText.Contains("Когда тебе станет лучше"))
            {
                colorContext = "undies";
                TranslationPlugin.logger.LogInfo($"[COLOR CONTEXT SET] Set to: undies (from: '{translatedText}')");
                ProcessPendingColorsForContext();
            }
            else if (translatedText.Contains("Wait—what color were her socks?!") || translatedText.Contains("Ahh, so refreshing~") ||
                    translatedText.Contains("Стой — какого цвета были носки?!") || translatedText.Contains("Ахх, как освежает~"))
            {
                colorContext = "socks";
                TranslationPlugin.logger.LogInfo($"[COLOR CONTEXT SET] Set to: socks (from: '{translatedText}')");
                processedColorsInSeries = 0;
            }
        }
    }

    static void ProcessText(GameObject gameObject, string value, int instanceId, System.Action<string> setText)
    {
        lock (translateLock)
        {
            try
            {
                if (setText == null)
                {
                    TranslationPlugin.logger.LogError("setText is null in ProcessText!");
                    return;
                }
                
                if (value == "Touch the circle to the border!" || value == "Touch the circle!")
                {
                    string touchTranslated = TranslationPlugin.GetTranslation(value);
                    if (touchTranslated == null)
                    {
                        touchTranslated = value == "Touch the circle to the border!" ? 
                            "Коснись круга внутри границы!" : "Коснись круга!";
                    }
                    
                    setText(touchTranslated);
                    TranslationPlugin.logger.LogInfo($"PROCESS TEXT: '{value}' -> '{touchTranslated}'");
                    return;
                }
                
                if (!string.IsNullOrEmpty(value) && value.Length >= 15 && !IsSystemText(value) && !value.Contains("UnityExplorer"))
                {
                    lastLongDialogue = value;
                    lastLongDialogueOriginal = value;
                    TranslationPlugin.logger.LogInfo($"[LAST DIALOGUE] Set to: '{value}'");
                }
                
                if (string.IsNullOrEmpty(value) || value.Length < 2 || IsSystemText(value) || IsPureNumbers(value))
                    return;

                if (ContainsCyrillic(value))
                    return;

                string translated = TranslationPlugin.GetTranslation(value);
                
                if (translated != null)
                {
                    UpdateColorContext(translated);
                    
                    setText(translated);
                    TranslationPlugin.logger.LogInfo($"TRANSLATED: '{value}' -> '{translated}'");
                }
                else
                {
                    setText(value);
                    currentTexts[instanceId] = value;
                    TranslationPlugin.LogUntranslatedText(value);
                }
            }
            catch (Exception e)
            {
                TranslationPlugin.logger.LogError($"Error in ProcessText: {e}");
                try
                {
                    if (setText != null)
                        setText(value);
                }
                catch (Exception ex)
                {
                    TranslationPlugin.logger.LogError($"Error in fallback setText: {ex}");
                }
            }
        }
    }

    static bool IsPureNumbers(string value)
    {
        return Regex.IsMatch(value.Trim(), @"^\d+$");
    }

    static bool ContainsCyrillic(string text)
    {
        foreach (char c in text)
        {
            if (c >= 'А' && c <= 'я') return true;
            if (c == 'ё' || c == 'Ё') return true;
        }
        return false;
    }

    static bool IsSystemText(string value)
    {
        if (value != null && value.Contains("UnityExplorer"))
            return true;
        
        string[] systemTexts = {
            "Name", "►", "▪", "1", "1,00", "i:", "NOT SET", "<notset>", 
            "Apply", "Value goes here", "...", "F7", "Time:", "timeScale",
            "||", "AutoCompleter", "Object Explorer", "Scene:", "Main",
            "Search and press enter...", "Update", "Auto-update", "Sibling Index",
            "Scene Loader", "Filter scene names...", "[Select a scene]", "Load (Single)",
            "Load (Additive)", "Searching for:", "Class filter:", "Child filter:",
            "Scene filter:", "Name contains:", "0 results", "Scene Explorer",
            "Object Search", "Inspector", "Mouse Inspect", "Close All", "C# Console",
            "Compile", "Reset", "Compile on Ctrl+R", "Suggestions", "Auto-indent",
            "Hooks", "Enter a class to add hooks to...", "Add Hooks", "Current Hooks",
            "Done", "Filter method names...", "Save and Return", "Cancel and Return",
            "Clipboard", "Current paste:", "Clear Clipboard", "not set", "Inspect",
            "Log", "Clear", "Open Log File", "Log Unity Debug?", "Options", "Save Options",
            "UI Inspector Results", "Inspect Under Mouse", "Mouse Inspector", "Mouse Position:",
            "No hits...", "UnityObject", "Any", "Enabled", "Delete", "Edit Hook Source",
            "Hook", "10:", "9:", "8:", "7:", "6:", "5:", "4:", "3:", "2:", "1:", "0:", "Up/Down to select", "Enter to use", "Esc to close"
        };
        
        return systemTexts.Contains(value);
    }
}
