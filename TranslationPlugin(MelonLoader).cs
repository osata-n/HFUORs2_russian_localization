using MelonLoader;
using HarmonyLib;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Il2CppTMPro;
using System.Text.RegularExpressions;
using UnityEngine;
using System;
using System.Threading;

[assembly: MelonInfo(typeof(TranslationPlugin), "Translation Mod", "1.0.0", "osata.n")]
[assembly: MelonGame(null, null)]

public class TranslationPlugin : MelonMod
{
    private static Dictionary<string, string> translations = new Dictionary<string, string>();
    private static HashSet<string> loggedTexts = new HashSet<string>();
    
    public override void OnInitializeMelon()
    {
        MelonLogger.Msg("Translation mod loaded!");
        
        string pluginPath = Path.GetDirectoryName(MelonAssembly.Assembly.Location);
        string translationsDir = Path.Combine(pluginPath, "translations");

        //  string untranslatedFile = Path.Combine(translationsDir, "untranslated.txt");
        
            // try
            // {
            //     if (File.Exists(untranslatedFile))
            //     {
            //         File.Delete(untranslatedFile);
            //         Logger.LogInfo("Файл untranslated.txt очищен при запуске.");
            //     }
            // }
            // catch (Exception ex)
            // {
            //     Logger.LogWarning($"Не удалось очистить untranslated.txt: {ex.Message}");
            // }
        
        LoadTranslations();
        
        HarmonyInstance.PatchAll(typeof(TextTranslationPatch));
        HarmonyInstance.PatchAll(typeof(SceneChangePatcher));
        
        MelonLogger.Msg("Patches applied!");
    }
    
    void LoadTranslations()
    {
        translations.Clear();
        TextTranslationPatch.ResetTranslationCache();
        
        Thread.Sleep(50);
        
        AddDefaultTranslations();
        
        string pluginPath = Path.GetDirectoryName(MelonAssembly.Assembly.Location);
        string translationsPath = Path.Combine(pluginPath, "translations");
        
        MelonLogger.Msg($"Looking for translations in: {translationsPath}");
        
        if (!Directory.Exists(translationsPath))
        {
            MelonLogger.Warning("Translations directory not found! Creating default...");
            CreateDefaultTranslations(translationsPath);
            return;
        }
        
        string[] translationFiles = Directory.GetFiles(translationsPath, "*.txt");
        
        foreach (string file in translationFiles)
        {
            LoadTranslationFile(file);
        }
        
        MelonLogger.Msg($"Loaded {translations.Count} translations from {translationFiles.Length} files");
        //  CreateUntranslatedFile(translationsPath);
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
        translations["Go to Mal-sook "] = "Подойти к Мал-сук";
        translations["Yeah, like… "] = "Да, типа…";
        translations["Alright, let’s see "] = "Хорошо, давайте посмотрим";
        translations["No, no, it’s fine… "] = "Нет, нет, все в порядке…";
        translations["Do you really think this mess is acceptable? "] = "Вы действительно думаете, что такой бардак приемлем?";
        translations["Mal-sook's Room"] = "Комната\nМал-сук";
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
        translations["— Man, the weather’s so nice today.\n— Huh? Isn’t that Mal-sook...?"] = "— Чувак, какая сегодня хорошая погода.\n— А? Это разве не Мал-сук...?";
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
        translations["- Cat! \n- Dog!"] = "- Кошка!\n- Собака!";
        translations["- Meow~ \n- Meow~"] = "- Мяу~\n- Мяу~";
        translations["- Black stockings! \n- Nude stockings!"] = "- Черные чулки!\n- Телесные чулки!";
        translations["(Business Plan Presentation)\nWann4 sleep?"] = "(Презентация бизнес-плана)\n“ВаннаФо слип”?";
        translations["- Nude stockings! \n- Black stockings!"] = "- Телесные чулки!\n- Черные чулки!";
        translations["I’ll consider today just a minor incident — "] = "Я сочту сегодняшнее лишь за мелкий инцидент —";
        translations["You shy sons of— may you all just rot forever, "] = "Вы, у#@%*и — чтоб вы все сгнили н#@#й,";
        translations["— Hahaha\n— Hahaha"] = "— Хахаха\n— Хахаха";
        translations["These days, I feel like "] = "В последнее время мне кажется,";
        translations["Smile"] = "Улыбнуться";
        translations["Current affection: 0%"] = "Текущая привязанность: 0%";
        translations["Current affection: %"] = "Текущая привязанность: %";
        translations[" If you don't have it, go get it!!!"] = "Если нет — сходи найди!!!";
        translations["Don't make me lose my temper—give me a cigarette~ "] = "Не доводи меня — дай сигареты~";
        translations["Huh?    "] = "М-м?";
        translations["Thank you for playing"] = "Спасибо за игру\nАвтор локализации - osata.n";
        translations["Because with the ex, "] = "Потому что с бывшей";
        translations["So at first, I really struggled "] = "Так что поначалу я очень мучилась,";
        translations["She wears light makeup, no flashy clothes... she’s more low-key.\n"] = "На ней легкий макияж, никакой броской одежды... она более сдержанная.";
        translations[" let’s go."] = "пойдем";
        translations["— Ah!\n— Oh!"] = "— Ай!\n— Ой!";
        translations["Take Care of Yourself First"] = "Позаботься сначала\nо себе";
        translations["Should I?"] = "А... наконец-то?";
        translations["Got it?"] = "Понял?";
        translations["- Code Blue, Code Blue.\n- Hurry, hurry."] = "— Синий код, синий код.\n— Быстрее, быстрее.";
        translations["Excuse me for a moment."] = "Прошу прощения, секунду.";
        translations["-Huh?\n- Uh…"] = "-А?\n-Э-э…";
        translations["—  Choi Wooseon! Choi Wooseon!\n— Ah…"] = "— Чхве У-Cон! Чхве У-Cон!\n— А…";
        translations["—  Choi Wooseon! Choi Wooseon!\n— For real?!"] = "\n— Серьезно?!";
        translations["— Kiss! Kiss! Kiss!\n— What the—"] = "— Целуй! Целуй! Целуй!\n— Что за—";
        translations["— Kiss! Kiss! Kiss!\n— Ah, seriously~!"] = "— Целуй! Целуй! Целуй!\n— Ай, ну серьезно~!";
        translations["— Kiss! Kiss! Kiss!\n— Why are you all like this!! Seriously!!"] = "— Целуй! Целуй! Целуй!\n— Ну что вы как маленькие!! Ну!!";
        translations["so please — we’d be honored "] = "поэтому, пожалуйста — мы будем очень рады,";
        translations["But really — I’m only bringing it up from a business standpoint "] = "Но, правда — я поднимаю это исключительно с деловой точки зрения,";
        translations["— I want to protect you with burning passion.\n— Woah.. woah—"] = "— Я хочу защищать тебя с пылающей страстью.\n— Ой.. ой—";
        translations["— I want to protect you with burning passion.\n— Woah— woah woah woah…!"] = "— Я хочу защищать тебя с пылающей страстью.\n— Ой— ой ой ой…!";
        translations["I swear, I was just passing by and saw trash so… "] = "Клянусь, я просто проходил мимо, увидел мусор и…";
        translations["Try Something Else"] = "Попробуй\nчто-нибудь иное";
        translations["I Support Your Dream"] = "Я поддержу\nтвою мечту";
        translations["I always knew the day would come "] = "Я всегда знал, что придет день,";
        translations["Your Legs Are Gorgeous"] = "У тебя\nшикарные ноги";
        translations["To bless your eyes with maximum sexy vibes. You’re welcome~ "] = "Чтобы благословить ваши взоры максимально сексуальной атмосферой. Так что жду~!";
        translations["thank you so, so much.\n"] = "огромное-преогромное спасибо.";
        translations["No way. "] = "Без шансов.>";
        translations["Love you, Bbanggyul~ "] = "Люблю тебя, Ппанггюль~";
        translations["Like, get married already! "] = "Типа, выходи уже замуж!";
        translations["Like, “Hey, what the hell is this?” "] = "Типа: “Эй, что это, чёрт возьми?”";
        translations["Every minute? "] = "Каждую минуту?";
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
                        MelonLogger.Msg($"Retrying to read {filePath} (attempt {attempts})");
                        Thread.Sleep(100);
                    }
                }
            }
            
            if (lines == null)
            {
                MelonLogger.Error($"Failed to read translation file: {filePath}");
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
                    
                    MelonLogger.Msg($"Loaded: '{key}' = '{value}'");
                }
            }
            
            MelonLogger.Msg($"Loaded/Updated {loadedCount} translations from: {Path.GetFileName(filePath)}");
        }
        catch (System.Exception e)
        {
            MelonLogger.Error($"Error loading translation file {filePath}: {e.Message}\n{e.StackTrace}");
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
        MelonLogger.Msg("Default translation files created!");
    }

        // void CreateUntranslatedFile(string translationsPath)
        // {
        //     string untranslatedPath = Path.Combine(translationsPath, "untranslated.txt");
        //     if (File.Exists(untranslatedPath))
        //     {
        //         File.Delete(untranslatedPath);
        //     }
        //     File.WriteAllText(untranslatedPath, "// Untranslated texts - copy these to appropriate translation files\n");
        // }
    
    public static string GetTranslation(string original)
    {
        if (translations.TryGetValue(original, out string translated))
            return translated;
        return null;
    }

        // public static void LogUntranslatedText(string text)
        // {
        //     if (!loggedTexts.Contains(text))
        //     {
        //         loggedTexts.Add(text);
                
        //         string pluginPath = Path.GetDirectoryName(typeof(TranslationPlugin).Assembly.Location);
        //         string translationsPath = Path.Combine(pluginPath, "translations");
        //         string untranslatedPath = Path.Combine(translationsPath, "untranslated.txt");
                
        //         try
        //         {
        //             File.AppendAllText(untranslatedPath, $"{text}=\n");
        //         }
        //         catch (System.Exception e)
        //         {
        //             logger.LogError($"Error writing to untranslated file: {e.Message}");
        //         }
        //     }
        // }
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
            MelonLogger.Msg($"Current bundle set to: {CurrentBundle}");
        }
    }
    
    public static void SetLastBundlePath(string path)
    {
        if (path != null && path.Contains("ccscripts_assets"))
        {
            LastBundlePath = path;
            CurrentBundle = Path.GetFileName(path);
            MelonLogger.Msg($"Bundle path: {path}, Current: {CurrentBundle}");
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
            MelonLogger.Msg($"[SCENE CHANGE] {lastSceneName} -> {sceneName}");
            
            TextTranslationPatch.ResetTranslationCache();
            lastSceneName = sceneName;
            
            MelonLogger.Msg($"Scene changed to: {sceneName}, cleared ALL contexts");
        }
    }
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(UnityEngine.SceneManagement.SceneManager), "LoadScene", new Type[] { typeof(int), typeof(UnityEngine.SceneManagement.LoadSceneMode) })]
    static void OnSceneLoad(int sceneBuildIndex, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        TextTranslationPatch.ResetTranslationCache();
        MelonLogger.Msg($"Scene changed to index: {sceneBuildIndex}, cleared text cache");
    }
}

[HarmonyPatch]
class TextTranslationPatch
{
    private static readonly object translateLock = new object();
    private static bool isSettingText = false;
    
    private static void ClearAllCache()
    {
        lock (translateLock)
        {
            MelonLogger.Msg($"[CLEAR ALL CACHE] Starting complete cache clear...");
            
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
            
            MelonLogger.Msg($"[CLEAR ALL CACHE] Complete! All caches and contexts reset");
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
            MelonLogger.Msg($"[CLEAR TEXTS] Starting clear...");
            
            currentTexts.Clear();
            CleanupInvalidPendingTranslations();
            
            if (recentTextLog.Count > 5)
            {
                recentTextLog = recentTextLog.Skip(recentTextLog.Count - 5).ToList();
            }
            
            if (!string.IsNullOrEmpty(colorContext))
            {
                MelonLogger.Msg($"[CLEAR TEXTS] Force resetting color context: {colorContext}");
                colorContext = "";
                processedColorsInSeries = 0;
                socksColorSeries.Clear();
                pendingColorTranslations.Clear();
                pendingColorValues.Clear();
                wasContextUsed = false;
            }
            
            MelonLogger.Msg($"[CLEAR TEXTS] Text cache cleared");
        }
    }
    
    public static void ResetColorContext()
    {
        lock (translateLock)
        {
            MelonLogger.Msg($"[RESET COLOR CONTEXT] Starting reset, current context: '{colorContext}'");
            
            colorContext = "";
            processedColorsInSeries = 0;
            socksColorSeries.Clear();
            pendingColorTranslations.Clear();
            pendingColorValues.Clear();
            
            lastContextUsedTime = System.DateTime.MinValue;
            lastSocksColorTime = System.DateTime.MinValue;
            wasContextUsed = false;
            
            MelonLogger.Msg($"[RESET COLOR CONTEXT] Complete, new context: '{colorContext}'");
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
                
                MelonLogger.Msg($"[SOCKS SERIES DEBUG] Count: {socksColorSeries.Count}, Time since last: {timeSinceLastSocksColor:F2}s, Threshold: 2.0f");
                
                if (timeSinceLastSocksColor > 2.0f && socksColorSeries.Count > 0)
                {
                    MelonLogger.Msg($"[SOCKS SERIES TIMEOUT] {timeSinceLastSocksColor:F2}s passed, processing series: {string.Join(", ", socksColorSeries)}");
                    ProcessSocksColorSeries();
                }
            }

            if (wasContextUsed && !string.IsNullOrEmpty(colorContext) && 
                (System.DateTime.Now - lastContextUsedTime).TotalSeconds > 0.5f)
            {
                MelonLogger.Msg($"[CONTEXT AUTO-RESET] 0.5s timeout: {colorContext}");
                colorContext = "";
                wasContextUsed = false;
                processedColorsInSeries = 0;
                socksColorSeries.Clear();
                
                if (pendingColorTranslations.Count > 0)
                {
                    MelonLogger.Msg($"[CONTEXT RESET] Clearing {pendingColorTranslations.Count} pending colors");
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
            
            MelonLogger.Msg($"{colorValue} -> PENDING for SOCKS series (ID: {instanceId})");
            MelonLogger.Msg($"[SOCKS SERIES] Added {colorValue}, series: {string.Join(", ", socksColorSeries)}");
            
            currentTexts[instanceId] = colorValue;
            
            if (socksColorSeries.Count >= 3)
            {
                MelonLogger.Msg($"[SOCKS SERIES] All 3 colors collected, processing immediately");
                ProcessSocksColorSeries();
            }
        }
        else
        {
            string translation = GetColorTranslation(colorValue, "socks");
            MelonLogger.Msg($"{colorValue} -> {translation} (SOCKS context)");
            
            SetTranslatedText(component, translation, instanceId);
            
            processedColorsInSeries++;
            MelonLogger.Msg($"[SOCKS] Processed {processedColorsInSeries}/3 colors");
            
            if (processedColorsInSeries >= 3)
            {
                MelonLogger.Msg($"[CONTEXT COMPLETE] All {processedColorsInSeries} socks colors processed");
                ResetColorContext();
                
                if (component.text == colorValue)
                {
                    string defaultTranslation = GetColorTranslation(colorValue, "");
                    MelonLogger.Msg($"[RECONTEXT] Re-translating {colorValue} -> {defaultTranslation} after context reset");
                    SetTranslatedText(component, defaultTranslation, instanceId);
                }
            }
        }
    }

    private static void ProcessSocksColorSeries()
    {
        if (pendingColorTranslations.Count == 0) 
        {
            MelonLogger.Msg($"[PROCESS SOCKS SERIES] No pending colors to process");
            return;
        }
        
        MelonLogger.Msg($"[SOCKS SERIES] Final processing: {string.Join(", ", socksColorSeries)}");
        
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
                MelonLogger.Msg($"[SOCKS SERIES] {pendingValue} -> {translation}");
            }
            
            pendingColorTranslations.Remove(pendingId);
            pendingColorValues.Remove(pendingId);
        }
        
        processedColorsInSeries += pendingIds.Count;
        
        if (processedColorsInSeries >= 3)
        {
            MelonLogger.Msg($"[CONTEXT COMPLETE] All socks colors processed");
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
            
            MelonLogger.Msg($"Processing {pendingColorTranslations.Count} pending colors for context: {colorContext}");
            
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
                    MelonLogger.Msg($"Pending {pendingValue} -> {translation} (context: {colorContext})");
                    
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
                MelonLogger.Msg($"[CONTEXT] Not enough dialogue context, returning UNKNOWN");
                return "UNKNOWN";
            }
            
            MelonLogger.Msg($"[CONTEXT ANALYSIS] Analyzing original: '{dialogueToAnalyze}'");

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

            if (dialogueToAnalyze.Contains("It was so great, so I'm really grateful. What about you, Yoo Man?") ||
                dialogueToAnalyze.Contains("Он был замечательным, и я очень благодарна. А для тебя, Юман?"))
            {
                return "FOR_YOU";
            }
            if (dialogueToAnalyze.Contains("Huh? You got stuck too?") ||
                dialogueToAnalyze.Contains("А? На тебе тоже?"))
            {
                return "STUCK";
            }
            
            int checkCount = Math.Min(3, recentTextLog.Count);
            for (int i = recentTextLog.Count - checkCount; i < recentTextLog.Count; i++)
            {
                if (i < 0) continue;
                
                string recentText = recentTextLog[i];
                if (string.IsNullOrEmpty(recentText)) continue;
                
                MelonLogger.Msg($"[RECENT CHECK {i}] '{recentText}'");
                
                if (recentText.Contains("stethoscope") || 
                    recentText.Contains("стетоскоп") ||
                    recentText.Contains("Could you help me check") ||
                    recentText.Contains("проверить стетоскоп"))
                {
                    MelonLogger.Msg($"[HISTORY FOUND] Stethoscope in recent history");
                    return "STETHOSCOPE";
                }
                
                if (recentText.Contains("swimming") || recentText.Contains("плавать") ||
                    recentText.Contains("athletic") || recentText.Contains("спорт"))
                {
                    MelonLogger.Msg($"[HISTORY FOUND] Sports in recent history");
                    return "SPORTS";
                }
                
                if (recentText.Contains("boarding house") || recentText.Contains("пансион") ||
                    recentText.Contains("share a floor") || recentText.Contains("Excuse me") ||
                    recentText.Contains("Why are you here") || recentText.Contains("Заселяться"))
                {
                    MelonLogger.Msg($"[HISTORY FOUND] Moving in recent history");
                    return "MOVING_IN";
                }
                
                if (recentText.Contains("What Are You Doing Here") || 
                    recentText.Contains("What are you doing here") ||
                    recentText.Contains("What do you want here"))
                {
                    MelonLogger.Msg($"[HISTORY FOUND] Presence question in recent history");
                    return "PRESENCE_QUESTION";
                }
                
                if (recentText.Contains("Wanna go") || recentText.Contains("Want to go") ||
                    recentText.Contains("together") || recentText.Contains("вместе"))
                {
                    MelonLogger.Msg($"[HISTORY FOUND] Invitation in recent history");
                    return "INVITATION";
                }
                
                if (recentText.Contains("curious about") || recentText.Contains("интерес к"))
                {
                    MelonLogger.Msg($"[HISTORY FOUND] Curious about in recent history");
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
                MelonLogger.Msg($"[CONTEXT DEFAULT] Has some context, defaulting to DEFAULT");
                return "DEFAULT";
            }
            else
            {
                MelonLogger.Msg($"[CONTEXT] Generic question, not enough for Me? translation");
                return "UNKNOWN";
            }
        }
    }
    
    private static string GetRightContext()
    {
        lock (translateLock)
        {
            string dialogueToAnalyze = lastLongDialogueOriginal;
            
            MelonLogger.Msg($"[RIGHT CONTEXT ANALYSIS] Analyzing: '{dialogueToAnalyze}'");
            
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

            if (dialogueToAnalyze.Contains("Hey, cheer up — you just seemed so tense, I was joking.") ||
                dialogueToAnalyze.Contains("Эй, да расслабься — ты так напряглась, я же пошутил."))
            {
                return "CHEER_UP_JOKING";
            }

            if (dialogueToAnalyze.Contains("Ugh, this sucks.") ||
                dialogueToAnalyze.Contains("Уф, это отстой."))
            {
                return "THIS_SUCKS";
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
                    MelonLogger.Msg($"Found MASSAGE context from last dialogue: '{lastLongDialogueOriginal}'");
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
                        MelonLogger.Msg($"Found MASSAGE context from recent history: '{recentText}'");
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
                        recentText.Contains("физиотерапия") ||
                        recentText.Contains("поработаю") ||
                        recentText.Contains("работать над"))
                    {
                        MelonLogger.Msg($"Found MASSAGE context from Russian translation: '{recentText}'");
                        return "MASSAGE";
                    }

                    if (recentText.Contains("It's my first stage since that incident, so I've been stressing out a lot...") ||
                        recentText.Contains("Это мое первое выступление после того инцидента, так что я очень переживаю..."))
                    {
                        MelonLogger.Msg($"Found PERFORMANCE_STRESS context from recent history: '{recentText}'");
                        return "PERFORMANCE_STRESS";
                    }
                }
            }
            
            MelonLogger.Msg($"No MASSAGE context found, using DEFAULT");
            return "DEFAULT";
        }
    }

    private static string GetApologyContext()
    {
        lock (translateLock)
        {
            MelonLogger.Msg($"[$] GetApologyContext called, recentTextLog count: {recentTextLog.Count}");
            
            if (recentTextLog.Count < 4)
            {
                MelonLogger.Msg($"[APOLOGY CONTEXT] Not enough logs: {recentTextLog.Count} < 4");
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

            string[] apology4Phrases = {
                "…Sorry… I just wanted to make you laugh…",
                "…Прости… Я просто хотел тебя рассмешить…"
            };
            
            if (CheckDialogForContext(dialog1, apology1Phrases) ||
                CheckDialogForContext(dialog2, apology1Phrases) ||
                CheckDialogForContext(dialog3, apology1Phrases))
            {
                MelonLogger.Msg($"[APOLOGY CONTEXT] Found CONTEXT_APOLOGY_1!");
                return "CONTEXT_APOLOGY_1";
            }
            
            if (CheckDialogForContext(dialog1, apology2Phrases) ||
                CheckDialogForContext(dialog2, apology2Phrases) ||
                CheckDialogForContext(dialog3, apology2Phrases))
            {
                MelonLogger.Msg($"[APOLOGY CONTEXT] Found CONTEXT_APOLOGY_2 (debt/after injury/story exaggerated)!");
                return "CONTEXT_APOLOGY_2";
            }
            
            if (CheckDialogForContext(dialog1, apology3Phrases) ||
                CheckDialogForContext(dialog2, apology3Phrases) ||
                CheckDialogForContext(dialog3, apology3Phrases))
            {
                MelonLogger.Msg($"[APOLOGY CONTEXT] Found CONTEXT_APOLOGY_3 (saving/apology expectation)!");
                return "CONTEXT_APOLOGY_3";
            }

            if (CheckDialogForContext(dialog1, apology4Phrases) ||
                CheckDialogForContext(dialog2, apology4Phrases) ||
                CheckDialogForContext(dialog3, apology4Phrases))
            {
                MelonLogger.Msg($"[APOLOGY CONTEXT] Found CONTEXT_APOLOGY_4 (wanted to make laugh)!");
                return "CONTEXT_APOLOGY_4";
            }
            
            MelonLogger.Msg($"[APOLOGY CONTEXT] No context found");
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
            
            MelonLogger.Msg($"[RIGHT?! CONTEXT] Last 3: '{string.Join(" | ", lastThree)}'");
            
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
                    MelonLogger.Msg($"[RIGHT?! CONTEXT] Found CONTEXT_RIGHT_EXCLAMATION in: {dialog}");
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
            MelonLogger.Msg($"[HAPPY NOW CONTEXT] Analyzing recent logs, count: {recentTextLog.Count}");
            
            if (recentTextLog.Count < 4)
            {
                MelonLogger.Msg($"[HAPPY NOW CONTEXT] Not enough logs: {recentTextLog.Count} < 4");
                return "UNKNOWN";
            }
            
            int startIndex = Math.Max(0, recentTextLog.Count - 6);
            int endIndex = recentTextLog.Count - 2;
            
            MelonLogger.Msg($"[HAPPY NOW CONTEXT] Checking dialogs from {startIndex} to {endIndex}:");
            
            int moreCount = 0;
            bool hasExclamation = false;
            
            for (int i = startIndex; i <= endIndex; i++)
            {
                string dialog = recentTextLog[i];
                if (dialog == null) continue;
                
                MelonLogger.Msg($"  [{i}]: '{dialog}'");
                
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
            
            MelonLogger.Msg($"[HAPPY NOW CONTEXT] Found 'More.' count: {moreCount}, hasExclamation: {hasExclamation}");
            
            if (moreCount >= 2 && hasExclamation)
            {
                MelonLogger.Msg($"[HAPPY NOW CONTEXT] Found CONTEXT_HAPPY_NOW_MORE (multiple More with exclamation)!");
                return "CONTEXT_HAPPY_NOW_MORE";
            }
            else if (moreCount >= 2)
            {
                MelonLogger.Msg($"[HAPPY NOW CONTEXT] Found CONTEXT_HAPPY_NOW_MORE (multiple More)!");
                return "CONTEXT_HAPPY_NOW_MORE";
            }
            
            MelonLogger.Msg($"[HAPPY NOW CONTEXT] No context found");
            return "UNKNOWN";
        }
    }

    private static string GetWhyContext()
    {
        lock (translateLock)
        {
            MelonLogger.Msg($"[WHY CONTEXT] Analyzing recent logs, count: {recentTextLog.Count}");
            
            if (recentTextLog.Count < 3)
            {
                MelonLogger.Msg($"[WHY CONTEXT] Not enough logs: {recentTextLog.Count} < 3");
                return "UNKNOWN";
            }
            
            int startIndex = Math.Max(0, recentTextLog.Count - 5);
            int endIndex = recentTextLog.Count - 2;
            
            MelonLogger.Msg($"[WHY CONTEXT] Checking dialogs from {startIndex} to {endIndex}:");
            
            bool hasActionRequest = false;
            bool hasGetInMyArmsandBathroom = false;
            bool hasDrinkingContext = false;
            bool hasChiefContext = false;
            
            for (int i = startIndex; i <= endIndex; i++)
            {
                string dialog = recentTextLog[i];
                if (dialog == null) continue;
                
                MelonLogger.Msg($"  [{i}]: '{dialog}'");
                
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
                    MelonLogger.Msg($"[WHY CONTEXT] Found action request: '{dialog}'");
                }
                
                if (dialog == "Get in my arms." ||
                    dialog == "Иди в мои объятия.")
                {
                    hasGetInMyArmsandBathroom = true;
                    MelonLogger.Msg($"[WHY CONTEXT] Found 'Get in my arms': '{dialog}'");
                }

                if (dialog == "Where are you going?" ||
                    dialog == "Huh? I’m going to the bathroom…")
                {
                    hasGetInMyArmsandBathroom = true;
                    MelonLogger.Msg($"[WHY CONTEXT] Found 'bathroom': '{dialog}'");
                }

                if (dialog == "Alright… shall we start drinking?" ||
                    dialog == "Ну что… пора выпить?" ||
                    dialog.Contains("shall we start drinking") ||
                    dialog.Contains("пора выпить"))
                {
                    hasDrinkingContext = true;
                    MelonLogger.Msg($"[WHY CONTEXT] Found drinking context: '{dialog}'");
                }

                if (dialog == "Chief, are you still awake?" ||
                    dialog == "Шеф, ты еще не спишь?")
                {
                    hasChiefContext = true;
                    MelonLogger.Msg($"[WHY CONTEXT] Found Chief context: '{dialog}'");
                }
            }
            
            if (hasGetInMyArmsandBathroom)
            {
                MelonLogger.Msg($"[WHY CONTEXT] Found CONTEXT_WHY_GET_IN_MY_ARMS_and_BATHROOM!");
                return "CONTEXT_WHY_GET_IN_MY_ARMS_and_BATHROOM";
            }
            else if (hasActionRequest)
            {
                MelonLogger.Msg($"[WHY CONTEXT] Found CONTEXT_WHY_ACTION_REQUEST!");
                return "CONTEXT_WHY_ACTION_REQUEST";
            }
            else if (hasDrinkingContext)
            {
                MelonLogger.Msg($"[WHY CONTEXT] Found CONTEXT_WHY_DRINKING!");
                return "CONTEXT_WHY_DRINKING";
            }
            else if (hasChiefContext)
            {
                MelonLogger.Msg($"[WHY CONTEXT] Found CONTEXT_WHY_CHIEF!");
                return "CONTEXT_WHY_CHIEF";
            }
            
            MelonLogger.Msg($"[WHY CONTEXT] No context found");
            return "UNKNOWN";
        }
    }

    private static string GetHoldOnContext()
    {
        lock (translateLock)
        {
            MelonLogger.Msg($"[HOLD ON CONTEXT] Analyzing recent logs, count: {recentTextLog.Count}");
            
            if (recentTextLog.Count < 3)
            {
                MelonLogger.Msg($"[HOLD ON CONTEXT] Not enough logs: {recentTextLog.Count} < 3");
                return "UNKNOWN";
            }
            
            int startIndex = Math.Max(0, recentTextLog.Count - 5);
            int endIndex = recentTextLog.Count - 2;
            
            MelonLogger.Msg($"[HOLD ON CONTEXT] Checking dialogs from {startIndex} to {endIndex}:");
            
            bool hasHairLoose = false;
            
            for (int i = startIndex; i <= endIndex; i++)
            {
                string dialog = recentTextLog[i];
                if (dialog == null) continue;
                
                MelonLogger.Msg($"  [{i}]: '{dialog}'");
                
                if (dialog == "Your hair came loose." || 
                    dialog.Contains("hair came loose") ||
                    dialog.Contains("волосы распустились"))
                {
                    hasHairLoose = true;
                    MelonLogger.Msg($"[HOLD ON CONTEXT] Found hair loose mention");
                }
            }
            
            if (hasHairLoose)
            {
                MelonLogger.Msg($"[HOLD ON CONTEXT] Found CONTEXT_HOLD_ON_HAIR_LOOSE!");
                return "CONTEXT_HOLD_ON_HAIR_LOOSE";
            }
            
            MelonLogger.Msg($"[HOLD ON CONTEXT] No context found");
            return "UNKNOWN";
        }
    }
    
    private static string GetArmContext()
    {
        lock (translateLock)
        {
            MelonLogger.Msg($"[ARM CONTEXT] Analyzing recent logs, count: {recentTextLog.Count}");
            
            if (recentTextLog.Count < 3)
            {
                MelonLogger.Msg($"[ARM CONTEXT] Not enough logs: {recentTextLog.Count} < 3");
                return "UNKNOWN";
            }
            
            int startIndex = Math.Max(0, recentTextLog.Count - 5);
            int endIndex = recentTextLog.Count - 2;
            
            MelonLogger.Msg($"[ARM CONTEXT] Checking dialogs from {startIndex} to {endIndex}:");
            
            bool hasBodyPartsContext = false;
            
            for (int i = startIndex; i <= endIndex; i++)
            {
                string dialog = recentTextLog[i];
                if (dialog == null) continue;
                
                MelonLogger.Msg($"  [{i}]: '{dialog}'");
                
                if (dialog == "Head" || dialog == "Голова" ||
                    dialog == "Knee" || dialog == "Колени")
                {
                    hasBodyPartsContext = true;
                    MelonLogger.Msg($"[ARM CONTEXT] Found body parts context: '{dialog}'");
                }
            }
            
            if (hasBodyPartsContext)
            {
                MelonLogger.Msg($"[ARM CONTEXT] Found CONTEXT_ARM_BODY_PARTS!");
                return "CONTEXT_ARM_BODY_PARTS";
            }
            
            MelonLogger.Msg($"[ARM CONTEXT] No context found");
            return "UNKNOWN";
        }
    }
    
    private static void ResetAreYouOkayContext()
    {
        lock (translateLock)
        {
            areYouOkayContext = "";
            MelonLogger.Msg($"Are you okay? context reset");
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

            MelonLogger.Msg($"[UGH CONTEXT] Previous dialogue: '{previousDialogue}'");

            if (previousDialogue != null && 
                (previousDialogue.Contains("used to fit you loose") || 
                previousDialogue.Contains("болталось") ||
                previousDialogue.Contains("fit you loose") ||
                previousDialogue.Contains("then,") && previousDialogue.Contains("fit")))
            {
                MelonLogger.Msg($"[UGH CONTEXT] Found CONTEXT_FIT_LOOSE!");
                return "CONTEXT_FIT_LOOSE";
            }

            if (!string.IsNullOrEmpty(lastLongDialogueOriginal) && 
                (lastLongDialogueOriginal.Contains("used to fit you loose") || 
                lastLongDialogueOriginal.Contains("fit you loose")))
            {
                MelonLogger.Msg($"[UGH CONTEXT] Found CONTEXT_FIT_LOOSE in lastLongDialogueOriginal!");
                return "CONTEXT_FIT_LOOSE";
            }

            MelonLogger.Msg($"[UGH CONTEXT] No specific context found");
            return "UNKNOWN";
        }
    }

    private static string GetHuhMeContext()
    {
        lock (translateLock)
        {
            MelonLogger.Msg($"[HUH ME CONTEXT] Analyzing context...");
            
            for (int i = Math.Max(0, recentTextLog.Count - 3); i < recentTextLog.Count; i++)
            {
                string dialog = recentTextLog[i];
                if (dialog == null) continue;
                
                MelonLogger.Msg($"  Recent [{i}]: '{dialog}'");
                
                if (dialog.Contains("whenever I see you") || 
                    dialog.Contains("когда я тебя вижу"))
                {
                    MelonLogger.Msg($"[HUH ME CONTEXT] Found CONTEXT_SEE_YOU!");
                    return "CONTEXT_SEE_YOU";
                }
                
                if (dialog == "Wanna give it a try, Yuman?" ||
                    dialog == "Хочешь попробовать, Юман?" ||
                    dialog.Contains("give it a try") ||
                    dialog.Contains("попробовать") ||
                    dialog.Contains("put this on") ||
                    dialog.Contains("наносить лак"))
                {
                    MelonLogger.Msg($"[HUH ME CONTEXT] Found CONTEXT_TRY_NAIL_POLISH!");
                    return "CONTEXT_TRY_NAIL_POLISH";
                }
            }
            
            MelonLogger.Msg($"[HUH ME CONTEXT] No specific context found");
            return "UNKNOWN";
        }
    }

    private static string GetHuhWhyContext()
    {
        lock (translateLock)
        {
            MelonLogger.Msg($"[HUH WHY CONTEXT] Analyzing recent logs, count: {recentTextLog.Count}");
            
            if (recentTextLog.Count < 3)
            {
                MelonLogger.Msg($"[HUH WHY CONTEXT] Not enough logs: {recentTextLog.Count} < 3");
                return "UNKNOWN";
            }
            
            int startIndex = Math.Max(0, recentTextLog.Count - 5);
            int endIndex = recentTextLog.Count - 2;
            
            MelonLogger.Msg($"[HUH WHY CONTEXT] Checking dialogs from {startIndex} to {endIndex}:");
            
            bool hasSmoothGuyContext = false;
            bool hasWrongLeague = false;
            bool hasDieAloneContext = false;
            
            for (int i = startIndex; i <= endIndex; i++)
            {
                string dialog = recentTextLog[i];
                if (dialog == null) continue;
                
                MelonLogger.Msg($"  [{i}]: '{dialog}'");
                
                if (dialog == "Even if a guy acted too smooth… I think I’d hate that." ||
                    dialog == "Даже если парень будет вести себя слишком гладко… думаю, мне бы это не понравилось." ||
                    dialog.Contains("acted too smooth") ||
                    dialog.Contains("ведет себя слишком гладко") ||
                    dialog.Contains("smooth guy") ||
                    dialog.Contains("гладкий парень"))
                {
                    hasSmoothGuyContext = true;
                    MelonLogger.Msg($"[HUH WHY CONTEXT] Found 'smooth guy' context: '{dialog}'");
                }

                if (dialog == "Sigh, ah... That's kinda out of my league?" ||
                    dialog == "*Вздох*... Это... не твое, наверное?")
                {
                    hasWrongLeague = true;
                    MelonLogger.Msg($"[HUH WHY CONTEXT] Found 'wrong league' context: '{dialog}'");
                }

                if (dialog == "Then... I'd rather just die alone." ||
                    dialog == "Тогда... я лучше умру в одиночестве." ||
                    dialog.Contains("die alone") ||
                    dialog.Contains("умру в одиночестве"))
                {
                    hasDieAloneContext = true;
                    MelonLogger.Msg($"[HUH WHY CONTEXT] Found 'die alone' context: '{dialog}'");
                }
            }
            
            if (hasSmoothGuyContext)
            {
                MelonLogger.Msg($"[HUH WHY CONTEXT] Found CONTEXT_HUH_WHY_SMOOTH_GUY!");
                return "CONTEXT_HUH_WHY_SMOOTH_GUY";
            }

            if (hasWrongLeague)
            {
                MelonLogger.Msg($"[HUH WHY CONTEXT] Found CONTEXT_HUH_WHY_WRONG_LEAGUE!");
                return "CONTEXT_HUH_WHY_WRONG_LEAGUE";
            }

            if (hasDieAloneContext)
            {
                MelonLogger.Msg($"[HUH WHY CONTEXT] Found CONTEXT_HUH_WHY_DIE_ALONE!");
                return "CONTEXT_HUH_WHY_DIE_ALONE";
            }
            
            MelonLogger.Msg($"[HUH WHY CONTEXT] No context found");
            return "UNKNOWN";
        }
    }

    private static string GetAhContext()
    {
        lock (translateLock)
        {
            MelonLogger.Msg($"[AH CONTEXT] Analyzing recent logs, count: {recentTextLog.Count}");
            
            if (recentTextLog.Count < 3)
            {
                MelonLogger.Msg($"[AH CONTEXT] Not enough logs: {recentTextLog.Count} < 3");
                return "UNKNOWN";
            }
            
            int startIndex = Math.Max(0, recentTextLog.Count - 5);
            int endIndex = recentTextLog.Count - 2;
            
            MelonLogger.Msg($"[AH CONTEXT] Checking dialogs from {startIndex} to {endIndex}:");
            
            bool hasMedicalContext = false;
            
            for (int i = startIndex; i <= endIndex; i++)
            {
                string dialog = recentTextLog[i];
                if (dialog == null) continue;
                
                MelonLogger.Msg($"  [{i}]: '{dialog}'");
                
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
                    dialog == "Юман! Скажи “аа~”" ||
                    dialog == "Yu-jeong" ||
                    dialog == "Sing while looking at me." ||
                    dialog == "When you came to see me perform.")
                {
                    hasMedicalContext = true;
                    MelonLogger.Msg($"[AH CONTEXT] Found medical context: '{dialog}'");
                }
            }
            
            if (hasMedicalContext)
            {
                MelonLogger.Msg($"[AH CONTEXT] Found CONTEXT_AH_FOOD!");
                return "CONTEXT_AH_FOOD";
            }
            
            MelonLogger.Msg($"[AH CONTEXT] No context found");
            return "UNKNOWN";
        }
    }

    private static string GetTalkContext()
    {
        lock (translateLock)
        {
            MelonLogger.Msg($"[TALK CONTEXT] Analyzing recent logs, count: {recentTextLog.Count}");
            
            if (recentTextLog.Count < 3)
            {
                MelonLogger.Msg($"[TALK CONTEXT] Not enough logs: {recentTextLog.Count} < 3");
                return "UNKNOWN";
            }
            
            int startIndex = Math.Max(0, recentTextLog.Count - 5);
            int endIndex = Math.Min(recentTextLog.Count - 1, recentTextLog.Count);
            
            MelonLogger.Msg($"[TALK CONTEXT] Checking dialogs from {startIndex} to {endIndex}:");
            
            bool hasRunAwayOption = false;
            
            for (int i = startIndex; i < endIndex; i++)
            {
                string dialog = recentTextLog[i];
                if (dialog == null) continue;
                
                MelonLogger.Msg($"  [{i}]: '{dialog}'");
                
                if (dialog == "Run Away" || dialog == "Убежать")
                {
                    hasRunAwayOption = true;
                    MelonLogger.Msg($"[TALK CONTEXT] Found 'Run Away' option: '{dialog}'");
                    break;
                }
            }
            
            if (hasRunAwayOption)
            {
                MelonLogger.Msg($"[TALK CONTEXT] Found CONTEXT_TALK_ACTION_CHOICE!");
                return "CONTEXT_TALK_ACTION_CHOICE";
            }
            
            MelonLogger.Msg($"[TALK CONTEXT] No context found");
            return "UNKNOWN";
        }
    }

    private static string GetWhatIsItContext()
    {
        lock (translateLock)
        {
            MelonLogger.Msg($"[WHAT IS IT CONTEXT] Analyzing recent logs, count: {recentTextLog.Count}");
            
            for (int i = Math.Max(0, recentTextLog.Count - 3); i < recentTextLog.Count; i++)
            {
                string dialog = recentTextLog[i];
                
                if (dialog == "Ah—jeez, you scared me!" || 
                    dialog == "Ах—боже, ты напугал меня!" ||
                    dialog.Contains("scared me") ||
                    dialog.Contains("напугал"))
                {
                    MelonLogger.Msg($"[WHAT IS IT CONTEXT] Found 'scared me' at index {i}, using CONTEXT_WHAT_IS_IT_SCARED");
                    return "CONTEXT_WHAT_IS_IT_SCARED";
                }
                
                if (dialog == "Wanna know my secret move for when I get scared?" ||
                    dialog == "Хочешь узнать мой секретный прием, когда страшно?" ||
                    dialog.Contains("secret move") ||
                    dialog.Contains("секретный прием"))
                {
                    MelonLogger.Msg($"[WHAT IS IT CONTEXT] Found 'secret move' at index {i}, using CONTEXT_WHAT_IS_IT_SECRET_MOVE");
                    return "CONTEXT_WHAT_IS_IT_SECRET_MOVE";
                }
            }
            
            MelonLogger.Msg($"[WHAT IS IT CONTEXT] No context found");
            return "UNKNOWN";
        }
    }

    private static string GetYouSureContext()
    {
        lock (translateLock)
        {
            MelonLogger.Msg($"[YOU SURE CONTEXT] Analyzing recent logs, count: {recentTextLog.Count}");
            
            for (int i = Math.Max(0, recentTextLog.Count - 3); i < recentTextLog.Count; i++)
            {
                string dialog = recentTextLog[i];
                if (dialog == "No—just… turn around for a sec." || 
                    dialog == "Нет—просто… отвернись на секунду." ||
                    dialog == "I’ll get up on my own." ||
                    dialog == "Я сама встану.")
                {
                    MelonLogger.Msg($"[YOU SURE CONTEXT] Found female context at index {i}, using CONTEXT_YOU_SURE_FEMALE");
                    return "CONTEXT_YOU_SURE_FEMALE";
                }
            }
            
            MelonLogger.Msg($"[YOU SURE CONTEXT] No context found");
            return "UNKNOWN";
        }
    }

    private static string GetAhSorryContext()
    {
        lock (translateLock)
        {
            MelonLogger.Msg($"[AH SORRY CONTEXT] Analyzing recent logs, count: {recentTextLog.Count}");
            
            for (int i = Math.Max(0, recentTextLog.Count - 3); i < recentTextLog.Count; i++)
            {
                string dialog = recentTextLog[i];
                if (dialog == "Uh… I think I can let go now." || 
                    dialog == "Э-э… Думаю, теперь можно отпустить.")
                {
                    MelonLogger.Msg($"[AH SORRY CONTEXT] Found 'let go' context at index {i}, using CONTEXT_AH_SORRY_LET_GO");
                    return "CONTEXT_AH_SORRY_LET_GO";
                }
            }
            
            MelonLogger.Msg($"[AH SORRY CONTEXT] No context found");
            return "UNKNOWN";
        }
    }

    private static string GetWhiteContext()
    {
        lock (translateLock)
        {
            MelonLogger.Msg($"[WHITE CONTEXT] Analyzing recent logs, count: {recentTextLog.Count}");
            
            for (int i = Math.Max(0, recentTextLog.Count - 3); i < recentTextLog.Count; i++)
            {
                string dialog = recentTextLog[i];
                if (dialog == "You were wearing white." || 
                    dialog == "На тебе был белый." ||
                    dialog.Contains("were wearing white") ||
                    dialog.Contains("был белый"))
                {
                    MelonLogger.Msg($"[WHITE CONTEXT] Found 'wearing white' at index {i}, using CONTEXT_WHITE_SINGULAR");
                    return "CONTEXT_WHITE_SINGULAR";
                }
            }
            
            MelonLogger.Msg($"[WHITE CONTEXT] No context found");
            return "UNKNOWN";
        }
    }

    private static string GetSoundsGoodContext()
    {
        lock (translateLock)
        {
            MelonLogger.Msg($"[SOUNDS GOOD CONTEXT] Analyzing recent logs, count: {recentTextLog.Count}");
            
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
                    MelonLogger.Msg($"[SOUNDS GOOD CONTEXT] Found group/entry context at index {i}, using CONTEXT_SOUNDS_GOOD_GROUP_ENTRY");
                    return "CONTEXT_SOUNDS_GOOD_GROUP_ENTRY";
                }
            }
            
            MelonLogger.Msg($"[SOUNDS GOOD CONTEXT] No context found");
            return "UNKNOWN";
        }
    }

    private static string GetTakeALookContext()
    {
        lock (translateLock)
        {
            MelonLogger.Msg($"[TAKE A LOOK CONTEXT] Analyzing recent logs, count: {recentTextLog.Count}");
            
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
                    MelonLogger.Msg($"[TAKE A LOOK CONTEXT] Found photo result context at index {i}, using CONTEXT_TAKE_A_LOOK_PHOTO");
                    return "CONTEXT_TAKE_A_LOOK_PHOTO";
                }
            }
            
            MelonLogger.Msg($"[TAKE A LOOK CONTEXT] No context found");
            return "UNKNOWN";
        }
    }

    private static string GetComingContext()
    {
        lock (translateLock)
        {
            MelonLogger.Msg($"[COMING CONTEXT] Analyzing recent logs, count: {recentTextLog.Count}");
            
            for (int i = Math.Max(0, recentTextLog.Count - 3); i < recentTextLog.Count; i++)
            {
                string dialog = recentTextLog[i];
                if (dialog == "Yu-jeong! Can you grab my socks?!" || 
                    dialog == "Ючжон! Подашь мне носки?!")
                {
                    MelonLogger.Msg($"[COMING CONTEXT] Found socks context at index {i}, using CONTEXT_COMING_SOCKS");
                    return "CONTEXT_COMING_SOCKS";
                }
            }
            
            MelonLogger.Msg($"[COMING CONTEXT] No context found");
            return "UNKNOWN";
        }
    }

    private static string GetHelloContext()
    {
        lock (translateLock)
        {
            MelonLogger.Msg($"[HELLO CONTEXT] Analyzing recent logs, count: {recentTextLog.Count}");
            
            for (int i = Math.Max(0, recentTextLog.Count - 3); i < recentTextLog.Count; i++)
            {
                string dialog = recentTextLog[i];
                if (dialog == "Oh! Wuseon, hey!" || 
                    dialog == "О! У Сон, привет!")
                {
                    MelonLogger.Msg($"[HELLO CONTEXT] Found Wuseon context at index {i}, using CONTEXT_HELLO_WUSEON");
                    return "CONTEXT_HELLO_WUSEON";
                }
            }
            
            MelonLogger.Msg($"[HELLO CONTEXT] No context found");
            return "UNKNOWN";
        }
    }

    private static string GetSmileContext()
    {
        lock (translateLock)
        {
            MelonLogger.Msg($"[SMILE CONTEXT] Analyzing recent logs, count: {recentTextLog.Count}");
            
            for (int i = Math.Max(0, recentTextLog.Count - 3); i < recentTextLog.Count; i++)
            {
                string dialog = recentTextLog[i];
                if (dialog == "Happiness" || 
                    dialog == "Счастье")
                {
                    MelonLogger.Msg($"[SMILE CONTEXT] Found Happiness context at index {i}, using CONTEXT_SMILE_HAPPINESS");
                    return "CONTEXT_SMILE_HAPPINESS";
                }
            }
            
            MelonLogger.Msg($"[SMILE CONTEXT] No context found");
            return "UNKNOWN";
        }
    }

    private static string GetShouldIContext()
    {
        lock (translateLock)
        {
            MelonLogger.Msg($"[SHOULD I CONTEXT] Analyzing recent logs, count: {recentTextLog.Count}");
            
            for (int i = Math.Max(0, recentTextLog.Count - 3); i < recentTextLog.Count; i++)
            {
                string dialog = recentTextLog[i];
                if (dialog == "You're too exhausted to make any decisions now." || 
                    dialog == "Сейчас ты слишком измотана, чтобы принимать решения.")
                {
                    MelonLogger.Msg($"[SHOULD I CONTEXT] Found exhaustion context at index {i}, using CONTEXT_SHOULD_I_EXHAUSTED");
                    return "CONTEXT_SHOULD_I_EXHAUSTED";
                }
            }
            
            MelonLogger.Msg($"[SHOULD I CONTEXT] No context found");
            return "UNKNOWN";
        }
    }

    private static string GetGotItQuestionContext()
    {
        lock (translateLock)
        {
            MelonLogger.Msg($"[GOT IT? CONTEXT] Analyzing recent logs, count: {recentTextLog.Count}");
            
            for (int i = Math.Max(0, recentTextLog.Count - 3); i < recentTextLog.Count; i++)
            {
                string dialog = recentTextLog[i];
                if (dialog == "So you have to meet me every time I come." || 
                    dialog == "Поэтому ты должна встречаться со мной каждый раз, как буду приходить.")
                {
                    MelonLogger.Msg($"[GOT IT? CONTEXT] Found meeting context at index {i}, using CONTEXT_GOT_IT_MEETING");
                    return "CONTEXT_GOT_IT_MEETING";
                }

                if (dialog == "It’s totally fine if the angle’s a little off like this." ||
                    dialog == "Даже если ракурс немного не такой, это нормально.")
                {
                    MelonLogger.Msg($"[GOT IT? CONTEXT] Found photo angle context at index {i}, using CONTEXT_GOT_IT_PHOTO_ANGLE");
                    return "CONTEXT_GOT_IT_PHOTO_ANGLE";
                }
            }
            
            MelonLogger.Msg($"[GOT IT? CONTEXT] No context found");
            return "UNKNOWN";
        }
    }

    private static string GetExcuseMeContext()
    {
        lock (translateLock)
        {
            MelonLogger.Msg($"[EXCUSE ME CONTEXT] Analyzing recent logs, count: {recentTextLog.Count}");
            
            for (int i = Math.Max(0, recentTextLog.Count - 3); i < recentTextLog.Count; i++)
            {
                string dialog = recentTextLog[i];
                if (dialog == "Yes — would you like to stand here?" || 
                    dialog == "Да — встанешь вот тут?")
                {
                    MelonLogger.Msg($"[EXCUSE ME CONTEXT] Found standing context at index {i}, using CONTEXT_EXCUSE_ME_STANDING");
                    return "CONTEXT_EXCUSE_ME_STANDING";
                }
            }
            
            MelonLogger.Msg($"[EXCUSE ME CONTEXT] No context found");
            return "UNKNOWN";
        }
    }

    private static string TranslateAffectionPhrase(string original)
    {
        System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"Current affection: (\d+)%");
        System.Text.RegularExpressions.Match match = regex.Match(original);
        
        if (match.Success)
        {
            string percentage = match.Groups[1].Value;
            return $"Текущая привязанность: {percentage}%";
        }
        
        return null;
    }

    private static void ProcessPendingFeedsHer(string context)
    {
        lock (translateLock)
        {
            if (pendingFeedsHerTranslations.Count == 0) return;
            
            MelonLogger.Msg($"Processing {pendingFeedsHerTranslations.Count} pending Feeds Her translations...");
            
            string translation = context == "YUNA" ? "Напоить ее" : "Покормить/Напоить";
            
            foreach (var pending in pendingFeedsHerTranslations.ToList())
            {
                int pendingId = pending.Key;
                TMP_Text pendingText = pending.Value;
                
                if (pendingText != null && pendingText.gameObject != null)
                {
                    pendingText.text = translation;
                    currentTexts[pendingId] = translation;
                    
                    MelonLogger.Msg($"Pending Feeds Her (ID: {pendingId}) -> {translation}");
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
            
            MelonLogger.Msg($"Processing {pendingMeTranslations.Count} pending Me? translations...");
            
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
                    
                    MelonLogger.Msg($"Pending Me? (ID: {pendingId}) -> {translation} (context: {context})");
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
            
            MelonLogger.Msg($"Processing {pendingMeTranslations.Count} pending Me? with DEFAULT context...");
            
            foreach (var pending in pendingMeTranslations.ToList())
            {
                int pendingId = pending.Key;
                TMP_Text pendingText = pending.Value;
                
                if (pendingText != null && pendingText.gameObject != null)
                {
                    SetTranslatedText(pendingText, "Я?", pendingId);
                    
                    MelonLogger.Msg($"Pending Me? (ID: {pendingId}) -> Я? (default fallback)");
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
            
            MelonLogger.Msg($"Processing {pendingRightTranslations.Count} pending Right? translations...");
            
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
                    
                    MelonLogger.Msg($"Pending Right? (ID: {pendingId}) -> {translation} (context: {context})");
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
                MelonLogger.Msg($"Clearing {pendingRightTranslations.Count} pending Right? translations");
                pendingRightTranslations.Clear();
            }
            
            if (pendingFeedsHerTranslations.Count > 0)
            {
                MelonLogger.Msg($"Clearing {pendingFeedsHerTranslations.Count} pending Feeds Her translations");
                pendingFeedsHerTranslations.Clear();
            }
            
            if (pendingColorTranslations.Count > 0)
            {
                MelonLogger.Msg($"Clearing {pendingColorTranslations.Count} pending color translations");
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
                        MelonLogger.Error($"Error in UI setText: {e}");
                    }
                };
                
                ProcessText(__instance.gameObject, value, instanceId, setText);
            }
            catch (Exception e)
            {
                MelonLogger.Error($"Error in TranslateUIText: {e}");
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

        if (value != null && value.StartsWith("Current affection: ") && value.Contains("%"))
        {
            string translated = TranslateAffectionPhrase(value);
            if (translated != null)
            {
                lock (translateLock)
                {
                    int instanceId = __instance.GetInstanceID();
                    SetTranslatedText(__instance, translated, instanceId);
                    MelonLogger.Msg($"AFFECTION TRANSLATED: '{value}' -> '{translated}'");
                }
                return;
            }
        }

        // 1.
        if (value == "Touch the circle to the border!" || value == "Touch the circle!")
        {
            MelonLogger.Msg($"DIRECT CATCH: Touch phrase detected: '{value}'");
            
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
                    
                    MelonLogger.Msg($"FORCE TRANSLATED: '{value}' -> '{translated}'");
                }
            }
            catch (Exception e)
            {
                MelonLogger.Error($"Error in force translation: {e}");
                isSettingText = false;
            }
            return;
        }
        
        // 2. "I’m sorry..."
        if (value == "I’m sorry..." || value == "I’m sorry…")
        {
            lock (translateLock)
            {
                MelonLogger.Msg($"=== I’M SORRY... DETECTED: '{value}' ===");
                
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
                MelonLogger.Msg($"Apology context: {apologyContext}");
                
                if (apologyContext == "CONTEXT_APOLOGY_1")
                {
                    SetTranslatedText(__instance, "Мне жаль...", instanceId);
                    MelonLogger.Msg($"I’m sorry... -> Мне жаль... (context 1: Why didn't you tell me?)");
                    return;
                }
                else if (apologyContext == "CONTEXT_APOLOGY_2")
                {
                    SetTranslatedText(__instance, "Прости...", instanceId);
                    MelonLogger.Msg($"I’m sorry... -> Прости... (context 2: debt/after injury)");
                    return;
                }
                else if (apologyContext == "CONTEXT_APOLOGY_3")
                {
                    SetTranslatedText(__instance, "Мне очень жаль...", instanceId);
                    MelonLogger.Msg($"I’m sorry... -> Мне очень жаль... (context 3: saving/apology expectation)");
                    return;
                }
                else if (apologyContext == "CONTEXT_APOLOGY_4")
                {
                    SetTranslatedText(__instance, "Прости...", instanceId);
                    MelonLogger.Msg($"I'm sorry... -> Прости... (context 4: wanted to make laugh)");
                    return;
                }
                else
                {
                    SetTranslatedText(__instance, "Простите...", instanceId);
                    MelonLogger.Msg($"I’m sorry... -> Простите... (default, no context)");
                    return;
                }
            }
        }
        
        // 3. "Right?!"
        if (value == "Right?!")
        {
            lock (translateLock)
            {
                MelonLogger.Msg($"=== RIGHT?! DETECTED: '{value}' ===");
                
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
                MelonLogger.Msg($"Right?! context: {rightExclamationContext}");
                
                if (rightExclamationContext == "CONTEXT_RIGHT_EXCLAMATION")
                {
                    SetTranslatedText(__instance, "А то?!", instanceId);
                    MelonLogger.Msg($"Right?! -> А то?! (context)");
                    return;
                }
                else
                {
                    SetTranslatedText(__instance, "Точно?!", instanceId);
                    MelonLogger.Msg($"Right?! -> Точно?! (default, no context)");
                    return;
                }
            }
        }
        
        // 4. "Happy now?"
        if (value == "Happy now?")
        {
            lock (translateLock)
            {
                MelonLogger.Msg($"=== HAPPY NOW? DETECTED: '{value}' ===");
                
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
                MelonLogger.Msg($"Happy now? context: {happyNowContext}");
                
                if (happyNowContext == "CONTEXT_HAPPY_NOW_MORE")
                {
                    SetTranslatedText(__instance, "Может хватит?", instanceId);
                    MelonLogger.Msg($"Happy now? -> Может хватит? (context: multiple 'More')");
                    return;
                }
                else
                {
                    SetTranslatedText(__instance, "Довольны теперь?", instanceId);
                    MelonLogger.Msg($"Happy now? -> Довольны теперь? (default, no context)");
                    return;
                }
            }
        }

        // 5. "Why?"
        if (value == "Why?")
        {
            lock (translateLock)
            {
                MelonLogger.Msg($"=== WHY? DETECTED: '{value}' ===");
                
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
                MelonLogger.Msg($"Why? context: {whyContext}");
                
                if (whyContext == "CONTEXT_WHY_GET_IN_MY_ARMS_and_BATHROOM")
                {
                    SetTranslatedText(__instance, "Зачем?", instanceId);
                    MelonLogger.Msg($"Why? -> Зачем? (context: Get in my arms)");
                    return;
                }
                else if (whyContext == "CONTEXT_WHY_ACTION_REQUEST")
                {
                    SetTranslatedText(__instance, "Зачем?", instanceId);
                    MelonLogger.Msg($"Why? -> Зачем? (context: action request)");
                    return;
                }
                else if (whyContext == "CONTEXT_WHY_DRINKING")
                {
                    SetTranslatedText(__instance, "Чего?", instanceId);
                    MelonLogger.Msg($"Why? -> Чего? (context: drinking)");
                    return;
                }
                else if (whyContext == "CONTEXT_WHY_CHIEF")
                {
                    SetTranslatedText(__instance, "А?", instanceId);
                    MelonLogger.Msg($"Why? -> А? (context: Chief)");
                    return;
                }
                else
                {
                    SetTranslatedText(__instance, "Почему?", instanceId);
                    MelonLogger.Msg($"Why? -> Почему? (default, no context)");
                    return;
                }
            }
        }
        
        // 6. "Hold on..."
        if (value == "Hold on..." || value == "Hold on…")
        {
            lock (translateLock)
            {
                MelonLogger.Msg($"=== HOLD ON... DETECTED: '{value}' ===");
                
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
                MelonLogger.Msg($"Hold on... context: {holdOnContext}");
                
                if (holdOnContext == "CONTEXT_HOLD_ON_HAIR_LOOSE")
                {
                    SetTranslatedText(__instance, "Замри...", instanceId);
                    MelonLogger.Msg($"Hold on... -> Замри... (context: hair loose)");
                    return;
                }
                else
                {
                    SetTranslatedText(__instance, "Постой...", instanceId);
                    MelonLogger.Msg($"Hold on... -> Постой... (default, no context)");
                    return;
                }
            }
        }

        // 7. "Arm"
        if (value == "Arm")
        {
            lock (translateLock)
            {
                MelonLogger.Msg($"=== ARM DETECTED: '{value}' ===");
                
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
                MelonLogger.Msg($"Arm context: {armContext}");
                
                if (armContext == "CONTEXT_ARM_BODY_PARTS")
                {
                    SetTranslatedText(__instance, "Руки", instanceId);
                    MelonLogger.Msg($"Arm -> Руки (context: body parts/medical)");
                    return;
                }
                else
                {
                    SetTranslatedText(__instance, "Рука", instanceId);
                    MelonLogger.Msg($"Arm -> Рука (default, no context)");
                    return;
                }
            }
        }

        // 8. "Ugh, seriously..."
        if (value == "Ugh, seriously..." || value == "Ugh, seriously…")
        {
            lock (translateLock)
            {
                MelonLogger.Msg($"=== UGH, SERIOUSLY... DETECTED: '{value}' ===");
                
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
                MelonLogger.Msg($"Ugh, seriously... context: {ughContext}");
                
                if (ughContext == "CONTEXT_FIT_LOOSE")
                {
                    SetTranslatedText(__instance, "Ну, скотина...", instanceId);
                    MelonLogger.Msg($"Ugh, seriously... -> Ну, скотина... (context: clothing used to fit loose)");
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
                    MelonLogger.Msg($"Ugh, seriously... -> {defaultTranslation} (default, no context)");
                    return;
                }
            }
        }

        // 9. "Huh? Me?"
        if (value == "Huh? Me?")
        {
            lock (translateLock)
            {
                MelonLogger.Msg($"=== HUH? ME? DETECTED: '{value}' ===");
                
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
                MelonLogger.Msg($"Huh? Me? context: {huhMeContext}");
                
                if (huhMeContext == "CONTEXT_SEE_YOU")
                {
                    SetTranslatedText(__instance, "А? Меня?", instanceId);
                    MelonLogger.Msg($"Huh? Me? -> А? Меня? (context: 'whenever I see you')");
                    return;
                }
                else if (huhMeContext == "CONTEXT_TRY_NAIL_POLISH")
                {
                    SetTranslatedText(__instance, "А? Я?", instanceId);
                    MelonLogger.Msg($"Huh? Me? -> А? Я? (context: nail polish/try)");
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
                    MelonLogger.Msg($"Huh? Me? -> {defaultTranslation} (default, no context)");
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
                MelonLogger.Msg($"=== HUH? WHY? DETECTED: '{value}' ===");
                
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
                MelonLogger.Msg($"Huh? Why? context: {huhWhyContext}");
                
                if (huhWhyContext == "CONTEXT_HUH_WHY_SMOOTH_GUY")
                {
                    SetTranslatedText(__instance, "А? Почему?", instanceId);
                    MelonLogger.Msg($"Huh? Why? -> А? Почему? (context: smooth guy)");
                    return;
                }
                else if (huhWhyContext == "CONTEXT_HUH_WHY_WRONG_LEAGUE")
                {
                    SetTranslatedText(__instance, "А? Почему?", instanceId);
                    MelonLogger.Msg($"Huh? Why? -> А? Почему? (context: wrong league)");
                    return;
                }
                else if (huhWhyContext == "CONTEXT_HUH_WHY_DIE_ALONE")
                {
                    SetTranslatedText(__instance, "А? Почему?", instanceId);
                    MelonLogger.Msg($"Huh? Why? -> А? Почему? (context: die alone)");
                    return;
                }
                else
                {
                    SetTranslatedText(__instance, "А? А что?", instanceId);
                    MelonLogger.Msg($"Huh? Why? -> А? А что? (default, no context)");
                    return;
                }
            }
        }

        // 12. "Ah~" (для ам - еды)
        if (value == "Ah~")
        {
            lock (translateLock)
            {
                MelonLogger.Msg($"=== AH~ DETECTED: '{value}' ===");
                
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
                MelonLogger.Msg($"Ah~ context: {ahContext}");
                
                if (ahContext == "CONTEXT_AH_FOOD")
                {
                    SetTranslatedText(__instance, "“Аа~”", instanceId);
                    MelonLogger.Msg($"Ah~ -> Аа~ (context: medical)");
                    return;
                }
                else
                {
                    SetTranslatedText(__instance, "Ах~", instanceId);
                    MelonLogger.Msg($"Ah~ -> Ах~ (default, no context)");
                    return;
                }
            }
        }

        // 13. "Talk"
        if (value == "Talk" || value == "Talk.")
        {
            lock (translateLock)
            {
                MelonLogger.Msg($"=== TALK DETECTED: '{value}' ===");
                
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
                MelonLogger.Msg($"Talk context: {talkContext}");
                
                if (talkContext == "CONTEXT_TALK_ACTION_CHOICE")
                {
                    SetTranslatedText(__instance, "Поговорить", instanceId);
                    MelonLogger.Msg($"Talk -> Поговорить (context: action choice with 'Run Away')");
                    return;
                }
                else
                {
                    SetTranslatedText(__instance, "Сказать", instanceId);
                    MelonLogger.Msg($"Talk -> Сказать (default, no context)");
                    return;
                }
            }
        }

        // 14. "What is it?"
        if (value == "What is it?")
        {
            lock (translateLock)
            {
                MelonLogger.Msg($"=== WHAT IS IT? DETECTED: '{value}' ===");
                
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
                MelonLogger.Msg($"What is it? context: {context}");
                
                if (context == "CONTEXT_WHAT_IS_IT_SCARED")
                {
                    SetTranslatedText(__instance, "Что такое?", instanceId);
                    MelonLogger.Msg($"What is it? -> Что такое? (context: after being scared)");
                    return;
                }
                else if (context == "CONTEXT_WHAT_IS_IT_SECRET_MOVE")
                {
                    SetTranslatedText(__instance, "Какой же?", instanceId);
                    MelonLogger.Msg($"What is it? -> Какой же? (context: secret move)");
                    return;
                }
                else
                {
                    SetTranslatedText(__instance, "Что это?", instanceId);
                    MelonLogger.Msg($"What is it? -> Что это? (default, no context)");
                    return;
                }
            }
        }

        // 15. "You sure?"
        if (value == "You sure?" || value == "You sure…" || value == "You sure...")
        {
            lock (translateLock)
            {
                MelonLogger.Msg($"=== YOU SURE? DETECTED: '{value}' ===");
                
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
                MelonLogger.Msg($"You sure? context: {context}");
                
                if (context == "CONTEXT_YOU_SURE_FEMALE")
                {
                    SetTranslatedText(__instance, "Уверена?", instanceId);
                    MelonLogger.Msg($"You sure? -> Уверена? (female context)");
                    return;
                }
                else
                {
                    SetTranslatedText(__instance, "Уверен?", instanceId);
                    MelonLogger.Msg($"You sure? -> Уверен? (default, male context)");
                    return;
                }
            }
        }

        // 16. "Ah! Sorry!"
        if (value == "Ah! Sorry!" || value == "Ah! Sorry…" || value == "Ah! Sorry...")
        {
            lock (translateLock)
            {
                MelonLogger.Msg($"=== AH! SORRY! DETECTED: '{value}' ===");
                
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
                MelonLogger.Msg($"Ah! Sorry! context: {context}");
                
                if (context == "CONTEXT_AH_SORRY_LET_GO")
                {
                    SetTranslatedText(__instance, "Ой! Прости!", instanceId);
                    MelonLogger.Msg($"Ah! Sorry! -> Ой! Прости! (context: let go)");
                    return;
                }
                else
                {
                    SetTranslatedText(__instance, "Ай! Простите!", instanceId);
                    MelonLogger.Msg($"Ah! Sorry! -> Ай! Простите! (default, no context)");
                    return;
                }
            }
        }

        // 17. "White?"
        if (value == "White?")
        {
            lock (translateLock)
            {
                MelonLogger.Msg($"=== WHITE? DETECTED: '{value}' ===");
                
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
                MelonLogger.Msg($"White? context: {context}");
                
                if (context == "CONTEXT_WHITE_SINGULAR")
                {
                    SetTranslatedText(__instance, "Белый?", instanceId);
                    MelonLogger.Msg($"White? -> Белый? (context: wearing white)");
                    return;
                }
                else
                {
                    SetTranslatedText(__instance, "Белые?", instanceId);
                    MelonLogger.Msg($"White? -> Белые? (default, plural)");
                    return;
                }
            }
        }

        // 18. "Sounds good."
        if (value == "Sounds good.")
        {
            lock (translateLock)
            {
                MelonLogger.Msg($"=== SOUNDS GOOD DETECTED: '{value}' ===");
                
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
                MelonLogger.Msg($"Sounds good. context: {context}");
                
                if (context == "CONTEXT_SOUNDS_GOOD_GROUP_ENTRY")
                {
                    SetTranslatedText(__instance, "Безусловно.", instanceId);
                    MelonLogger.Msg($"Sounds good. -> Безусловно. (context: group entry)");
                    return;
                }
                else
                {
                    SetTranslatedText(__instance, "Без проблем.", instanceId);
                    MelonLogger.Msg($"Sounds good. -> Без проблем. (default)");
                    return;
                }
            }
        }

        // 19. "Take a look."
        if (value == "Take a look.")
        {
            lock (translateLock)
            {
                MelonLogger.Msg($"=== TAKE A LOOK DETECTED: '{value}' ===");
                
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
                MelonLogger.Msg($"Take a look. context: {context}");
                
                if (context == "CONTEXT_TAKE_A_LOOK_PHOTO")
                {
                    SetTranslatedText(__instance, "Посмотри.", instanceId);
                    MelonLogger.Msg($"Take a look. -> Посмотри. (context: photo result)");
                    return;
                }
                else
                {
                    SetTranslatedText(__instance, "Дай гляну.", instanceId);
                    MelonLogger.Msg($"Take a look. -> Дай гляну. (default)");
                    return;
                }
            }
        }

        // 20. "Coming!"
        if (value == "Coming!")
        {
            lock (translateLock)
            {
                MelonLogger.Msg($"=== COMING! DETECTED: '{value}' ===");
                
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
                MelonLogger.Msg($"Coming! context: {context}");
                
                if (context == "CONTEXT_COMING_SOCKS")
                {
                    SetTranslatedText(__instance, "Сейчас!", instanceId);
                    MelonLogger.Msg($"Coming! -> Сейчас! (context: socks request)");
                    return;
                }
                else
                {
                    SetTranslatedText(__instance, "Иду!", instanceId);
                    MelonLogger.Msg($"Coming! -> Иду! (default)");
                    return;
                }
            }
        }

        // 21. "Hello..."
        if (value == "Hello...")
        {
            lock (translateLock)
            {
                MelonLogger.Msg($"=== HELLO... DETECTED: '{value}' ===");
                
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
                MelonLogger.Msg($"Hello... context: {context}");
                
                if (context == "CONTEXT_HELLO_WUSEON")
                {
                    SetTranslatedText(__instance, "Привет...", instanceId);
                    MelonLogger.Msg($"Hello... -> Привет... (context: Wuseon greeting)");
                    return;
                }
                else
                {
                    SetTranslatedText(__instance, "Алло...", instanceId);
                    MelonLogger.Msg($"Hello... -> Алло... (default)");
                    return;
                }
            }
        }

        // 22. "Smile"
        if (value == "Smile" || value == "Smile." || value == "Smile!")
        {
            lock (translateLock)
            {
                MelonLogger.Msg($"=== SMILE DETECTED: '{value}' ===");
                
                int instanceId = __instance.GetInstanceID();
                
                if (!string.IsNullOrEmpty(value) && !IsSystemText(value))
                {
                    if (recentTextLog.Count > 20)
                    {
                        recentTextLog.RemoveAt(0);
                    }
                    recentTextLog.Add(value);
                }
                
                string context = GetSmileContext();
                MelonLogger.Msg($"Smile context: {context}");
                
                if (context == "CONTEXT_SMILE_HAPPINESS")
                {
                    SetTranslatedText(__instance, "Улыбка", instanceId);
                    MelonLogger.Msg($"Smile -> Улыбка (context: Happiness)");
                    return;
                }
                else
                {
                    SetTranslatedText(__instance, "Улыбнуться", instanceId);
                    MelonLogger.Msg($"Smile -> Улыбнуться (default)");
                    return;
                }
            }
        }

        // 23. "Should I?"
        if (value == "Should I?" || value == "Should I…" || value == "Should I...")
        {
            lock (translateLock)
            {
                MelonLogger.Msg($"=== SHOULD I? DETECTED: '{value}' ===");
                
                int instanceId = __instance.GetInstanceID();
                
                if (!string.IsNullOrEmpty(value) && !IsSystemText(value))
                {
                    if (recentTextLog.Count > 20)
                    {
                        recentTextLog.RemoveAt(0);
                    }
                    recentTextLog.Add(value);
                }
                
                string context = GetShouldIContext();
                MelonLogger.Msg($"Should I? context: {context}");
                
                if (context == "CONTEXT_SHOULD_I_EXHAUSTED")
                {
                    SetTranslatedText(__instance, "Разве?", instanceId);
                    MelonLogger.Msg($"Should I? -> Разве? (context: exhausted)");
                    return;
                }
                else
                {
                    SetTranslatedText(__instance, "А... наконец-то?", instanceId);
                    MelonLogger.Msg($"Should I? -> А... наконец-то? (default)");
                    return;
                }
            }
        }

        // 24 "Got it?"
        if (value == "Got it?")
        {
            lock (translateLock)
            {
                MelonLogger.Msg($"=== GOT IT? DETECTED: '{value}' ===");
                
                int instanceId = __instance.GetInstanceID();
                
                if (!string.IsNullOrEmpty(value) && !IsSystemText(value))
                {
                    if (recentTextLog.Count > 20)
                    {
                        recentTextLog.RemoveAt(0);
                    }
                    recentTextLog.Add(value);
                }
                
                string context = GetGotItQuestionContext();
                MelonLogger.Msg($"Got it? context: {context}");
                
                if (context == "CONTEXT_GOT_IT_MEETING")
                {
                    SetTranslatedText(__instance, "Поняла?", instanceId);
                    MelonLogger.Msg($"Got it? -> Поняла? (context: meeting)");
                    return;
                }
                else if (context == "CONTEXT_GOT_IT_PHOTO_ANGLE")
                {
                    SetTranslatedText(__instance, "Поняла?", instanceId);
                    MelonLogger.Msg($"Got it? -> Поняла? (context: photo angle)");
                    return;
                }
                else
                {
                    string defaultTranslation = TranslationPlugin.GetTranslation(value);
                    if (defaultTranslation == null)
                    {
                        defaultTranslation = "Понял?";
                    }
                    SetTranslatedText(__instance, defaultTranslation, instanceId);
                    MelonLogger.Msg($"Got it? -> {defaultTranslation} (default, no context)");
                    return;
                }
            }
        }

        // 25. "Excuse me for a moment."
        if (value == "Excuse me for a moment." || value == "Excuse me for a moment…" || value == "Excuse me for a moment...")
        {
            lock (translateLock)
            {
                MelonLogger.Msg($"=== EXCUSE ME FOR A MOMENT DETECTED: '{value}' ===");
                
                int instanceId = __instance.GetInstanceID();
                
                if (!string.IsNullOrEmpty(value) && !IsSystemText(value))
                {
                    if (recentTextLog.Count > 20)
                    {
                        recentTextLog.RemoveAt(0);
                    }
                    recentTextLog.Add(value);
                }
                
                string context = GetExcuseMeContext();
                MelonLogger.Msg($"Excuse me for a moment. context: {context}");
                
                if (context == "CONTEXT_EXCUSE_ME_STANDING")
                {
                    SetTranslatedText(__instance, "Погоди, секунду.", instanceId);
                    MelonLogger.Msg($"Excuse me for a moment. -> Погоди, секунду. (context: standing)");
                    return;
                }
                else
                {
                    SetTranslatedText(__instance, "Прошу прощения, секунду.", instanceId);
                    MelonLogger.Msg($"Excuse me for a moment. -> Прошу прощения, секунду. (default)");
                    return;
                }
            }
        }

        // 26. Остальной код...
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
                    MelonLogger.Msg($"5s timeout reached for pending Me?, processing with default");
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
                    MelonLogger.Msg("=== ТЕКСТ ОБНАРУЖЕН ===");
                    MelonLogger.Msg($"Текст: '{value}'");
                    MelonLogger.Msg($"Текущий бандл: {BundleTracker.CurrentBundle ?? "NULL"}");
                    MelonLogger.Msg($"Имя сцены: {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}");
                    MelonLogger.Msg($"Объект: {__instance.gameObject.name}");
                    MelonLogger.Msg($"Текущий контекст: {colorContext}");
                    MelonLogger.Msg($"Контекст Are you okay?: {areYouOkayContext}");
                    MelonLogger.Msg($"Контекст использован: {wasContextUsed}");
                    MelonLogger.Msg($"Цветов в серии: {processedColorsInSeries}");
                    
                    Transform parent = __instance.transform.parent;
                    while (parent != null)
                    {
                        MelonLogger.Msg($"Родитель: {parent.name}");
                        parent = parent.parent;
                    }
                    MelonLogger.Msg("======================");
                }

                if (IsYuNaSceneMarker(value))
                {
                    MelonLogger.Msg($"YUNA SCENE MARKER DETECTED: '{value}'");
                    ProcessPendingFeedsHer("YUNA");
                }

                if (value == "Doctor" || value == "Доктор")
                {
                    MelonLogger.Msg($"DOCTOR MARKER DETECTED - checking if we should change translation");

                    bool hasPresenceQuestionInHistory = false;
                    foreach (string log in recentTextLog)
                    {
                        if (log.Contains("What Are You Doing Here") || 
                            log.Contains("What are you doing here") ||
                            log.Contains("What do you want here"))
                        {
                            hasPresenceQuestionInHistory = true;
                            MelonLogger.Msg($"Found presence question in history: {log}");
                            break;
                        }
                    }
                    
                    if (hasPresenceQuestionInHistory && pendingMeTranslations.Count > 0)
                    {
                        MelonLogger.Msg($"Changing {pendingMeTranslations.Count} pending 'Я?' to 'Обо мне?' due to doctor marker");
                        
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
                                    MelonLogger.Msg($"Changed pending Me? (ID: {pendingId}) to 'Обо мне?'");
                                    
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
                        MelonLogger.Msg($"Scene marker detected, clearing pending translations");
                        ClearPendingTranslations();
                    }
                    ResetAreYouOkayContext();
                }

                if ((System.DateTime.Now - lastMeDetection).TotalSeconds > 30 && 
                    (pendingRightTranslations.Count > 0))
                {
                    MelonLogger.Msg($"30s timeout reached, clearing pending Right? translations");
                    pendingRightTranslations.Clear();
                }

                if (value == "Feeds Her")
                {
                    MelonLogger.Msg($"Feeds Her detected (ID: {instanceId})");
                    
                    string detectedScene = DetectSceneFromHistory();
                    MelonLogger.Msg($"Detected scene: {detectedScene}");
                    
                    if (detectedScene == "GYURI")
                    {
                        SetTranslatedText(__instance, "Накормить ее", instanceId);
                        MelonLogger.Msg("Feeds Her -> Накормить ее (GyuRi scene)");
                        return;
                    }
                    else if (detectedScene == "YUNA")
                    {
                        SetTranslatedText(__instance, "Напоить ее", instanceId);
                        MelonLogger.Msg("Feeds Her -> Напоить ее (YuNa scene)");
                        return;
                    }
                    else
                    {
                        pendingFeedsHerTranslations[instanceId] = __instance;
                        MelonLogger.Msg($"Feeds Her -> PENDING (unknown scene, saved ID: {instanceId})");
                        
                        currentTexts[instanceId] = value;
                        return;
                    }
                }

                if (value == "Got it." || value == "Got it!")
                {
                    lock (translateLock)
                    {
                        MelonLogger.Msg($"=== GOT IT DETECTED: '{value}' ===");
                        
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
                        bool isWorkFeedbackContext = false;
                        
                        int checkCount = Math.Min(3, recentTextLog.Count);
                        for (int i = Math.Max(0, recentTextLog.Count - checkCount); i < recentTextLog.Count; i++)
                        {
                            string recentText = recentTextLog[i];
                            if (string.IsNullOrEmpty(recentText)) continue;
                            
                            MelonLogger.Msg($"  Recent [{i}]: '{recentText}'");
                            
                            if (recentText == "Watching you work at the hospital… it always looks so exhausting." ||
                                recentText == "Смотря, как ты работаешь в больнице… всегда кажется, что это так изматывает." ||
                                recentText == "Make sure you really take it easy today!" ||
                                recentText == "Обязательно как следует отдохни сегодня!" ||
                                recentText == "You too, Yuman—don’t overdo it, okay?" ||
                                recentText == "Ты тоже, Юман — не переусердствуй, ладно?")
                            {
                                isFemaleSpeakerContext = true;
                                MelonLogger.Msg($"  -> Yuman GotIt context detected (exact phrase match)");
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
                                MelonLogger.Msg($"  -> Caught context detected (exact phrase match)");
                                break;
                            }
                            
                            if (recentText == "Alright, deal?" ||
                                recentText == "Ладно, договорились?")
                            {
                                isAgreementContext = true;
                                MelonLogger.Msg($"  -> Agreement context detected (school beauty)");
                                break;
                            }

                            if (recentText == "I’ll send you a written summary with feedback later today." ||
                                recentText == "Я пришлю тебе письменный ответ с комментариями сегодня, позднее.")
                            {
                                isWorkFeedbackContext = true;
                                MelonLogger.Msg($"  -> Work feedback context detected");
                            }
                        }
                        
                        string translation;
                        if (value == "Got it.")
                        {
                            if (isFemaleSpeakerContext)
                            {
                                translation = "Принял.";
                                MelonLogger.Msg($"Got it. -> Принял. (Yuman GotIt context)");
                            }
                            else if (isAgreementContext)
                            {
                                translation = "Договорились.";
                                MelonLogger.Msg($"Got it. -> Договорились. (agreement context)");
                            }
                            else
                            {
                                translation = "Поняла.";
                                MelonLogger.Msg($"Got it. -> Поняла. (default)");
                            }
                        }
                        else
                        {
                            if (isCaughtContext)
                            {
                                translation = "Отлично!";
                                MelonLogger.Msg($"Got it! -> Отлично! (caught exact context)");
                            }
                            else if (isWorkFeedbackContext)
                            {
                                translation = "Отлично!";
                                MelonLogger.Msg($"Got it! -> Отлично! (work feedback context)");
                            }
                            else
                            {
                                translation = "Попалась!";
                                MelonLogger.Msg($"Got it! -> Попалась! (default)");
                            }
                        }
                        
                        SetTranslatedText(__instance, translation, instanceId);
                        return;
                    }
                }

                if (value == "You sure about that?")
                {
                    MelonLogger.Msg($"=== YOU SURE ABOUT THAT? DETECTED ===");
                    
                    bool isFemaleSpeakerContext = false;
                    
                    int checkCount = Math.Min(3, recentTextLog.Count);
                    for (int i = Math.Max(0, recentTextLog.Count - checkCount); i < recentTextLog.Count; i++)
                    {
                        string recentText = recentTextLog[i];
                        if (string.IsNullOrEmpty(recentText)) continue;
                        
                        MelonLogger.Msg($"  Recent [{i}]: '{recentText}'");
                        
                        if (recentText == "I definitely saw it—" ||
                            recentText == "Я точно видела—" ||
                            recentText == "trust me." ||
                            recentText == "поверь мне." ||
                            recentText == "Going with card 3, then?" ||
                            recentText == "Значит, выбираем карту 3?")
                        {
                            isFemaleSpeakerContext = true;
                            MelonLogger.Msg($"  -> Female speaker context detected (exact phrase match)");
                            break;
                        }
                    }
                    
                    string translation;
                    if (isFemaleSpeakerContext)
                    {
                        translation = "Уверена?";
                        MelonLogger.Msg($"You sure about that? -> Уверена? (female speaker context)");
                    }
                    else
                    {
                        translation = "Уверен?";
                        MelonLogger.Msg($"You sure about that? -> Уверен? (default)");
                    }
                    
                    SetTranslatedText(__instance, translation, instanceId);
                    return;
                }

                if (value == "Me?")
                {
                    lastMeDetection = System.DateTime.Now;
                    
                    MelonLogger.Msg($"=== ME? DETECTED ===");
                    MelonLogger.Msg($"Last dialogue (original): '{lastLongDialogueOriginal}'");
                    MelonLogger.Msg($"Last dialogue (translated): '{lastLongDialogue}'");
                    
                    string context = GetMeContext();
                    MelonLogger.Msg($"Me? context: {context}");
                    
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
                            MelonLogger.Msg($"Found stethoscope in recent history [{i}]: {log}");
                        }
                        
                        if (log.Contains("What Are You Doing Here") || 
                            log.Contains("What are you doing here") ||
                            log.Contains("What do you want here"))
                        {
                            hasPresenceQuestionInHistory = true;
                            MelonLogger.Msg($"Found presence question in recent history [{i}]: {log}");
                        }
                    }
                    
                    if (context == "STETHOSCOPE" || context == "MOVING_IN" || 
                        context == "SPORTS" || context == "TOGETHER" || context == "INVITATION" ||
                        hasStethoscopeInHistory)
                    {
                        SetTranslatedText(__instance, "Я?", instanceId);
                        MelonLogger.Msg($"Me? -> Я? ({context} context, stethoscope in recent: {hasStethoscopeInHistory})");
                        return;
                    }
                    else if (context == "ABOUT_DOCTOR" || context == "PRESENCE_QUESTION")
                    {
                        SetTranslatedText(__instance, "Обо мне?", instanceId);
                        MelonLogger.Msg($"Me? -> Обо мне? ({context} context)");
                        return;
                    }
                    else if (context == "FOR_YOU")
                    {
                        SetTranslatedText(__instance, "Для меня?", instanceId);
                        MelonLogger.Msg($"Me? -> Для меня? (context: What for you)");
                        return;
                    }
                    else if (context == "STUCK")
                    {
                        SetTranslatedText(__instance, "На мне?", instanceId);
                        MelonLogger.Msg($"Me? -> На мне? (context: stuck)");
                        return;
                    }
                    
                    if (hasPresenceQuestionInHistory)
                    {
                        SetTranslatedText(__instance, "Обо мне?", instanceId);
                        MelonLogger.Msg("Me? -> Обо мне? (presence question in recent history)");
                        return;
                    }
                    
                    if (context == "DEFAULT")
                    {
                        SetTranslatedText(__instance, "Я?", instanceId);
                        MelonLogger.Msg("Me? -> Я? (default context with enough history)");
                        return;
                    }
                    else if (context == "UNKNOWN" || context == "DEFAULT")
                    {
                        SetTranslatedText(__instance, "Я?", instanceId);
                        MelonLogger.Msg($"Me? -> Я? ({context} context, default translation)");
                        return;
                    }
                    else
                    {
                        SetTranslatedText(__instance, "Я?", instanceId);
                        MelonLogger.Msg("Me? -> Я? (fallback)");
                        return;
                    }
                }

                if (value == "Here you go.")
                {
                    MelonLogger.Msg($"=== HERE YOU GO DETECTED ===");
                    
                    bool isInformalContext = false;
                    
                    int checkCount = Math.Min(3, recentTextLog.Count);
                    for (int i = Math.Max(0, recentTextLog.Count - checkCount); i < recentTextLog.Count; i++)
                    {
                        string recentText = recentTextLog[i];
                        if (string.IsNullOrEmpty(recentText)) continue;
                        
                        MelonLogger.Msg($"  Recent [{i}]: '{recentText}'");
                        
                        if (recentText == "Thank you." ||
                            recentText == "Спасибо." ||
                            recentText == "Let’s give the gentleman the honor." ||
                            recentText == "Прошу джентльмена принять честь!" ||
                            recentText == "Yes, just a moment please." ||
                            recentText == "Да, момент, пожалуйста.")
                        {
                            isInformalContext = true;
                            MelonLogger.Msg($"  -> Informal context detected (exact phrase match)");
                            break;
                        }
                    }
                    
                    string translation;
                    if (isInformalContext)
                    {
                        translation = "Держи.";
                        MelonLogger.Msg($"Here you go. -> Держи. (informal context)");
                    }
                    else
                    {
                        translation = "Держите.";
                        MelonLogger.Msg($"Here you go. -> Держите. (default/formal)");
                    }
                    
                    SetTranslatedText(__instance, translation, instanceId);
                    return;
                }

                if (value == "Right?")
                {
                    MelonLogger.Msg($"=== RIGHT? DETECTED ===");
                    MelonLogger.Msg($"Last dialogue (original): '{lastLongDialogueOriginal}'");
                    MelonLogger.Msg($"Last dialogue (translated): '{lastLongDialogue}'");
                    
                    string context = GetRightContext();
                    MelonLogger.Msg($"Right? context: {context}");
                    
                    if (context == "CONFIRMATION")
                    {
                        SetTranslatedText(__instance, "Точно?", instanceId);
                        MelonLogger.Msg("Right? -> Точно? (confirmation context)");
                        return;
                    }
                    else if (context == "AGREEMENT")
                    {
                        SetTranslatedText(__instance, "Да?", instanceId);
                        MelonLogger.Msg("Right? -> Да? (agreement context)");
                        return;
                    }
                    else if (context == "NORMALITY")
                    {
                        SetTranslatedText(__instance, "Верно?", instanceId);
                        MelonLogger.Msg("Right? -> Верно? (normality context)");
                        return;
                    }
                    else if (context == "CHEER_UP_JOKING")
                    {
                        SetTranslatedText(__instance, "Правда?", instanceId);
                        MelonLogger.Msg("Right? -> Правда? (cheer up/joking context)");
                        return;
                    }
                    else if (context == "THIS_SUCKS")
                    {
                        SetTranslatedText(__instance, "Я права?", instanceId);
                        MelonLogger.Msg("Right? -> Я права? (this sucks context)");
                        return;
                    }
                    else
                    {
                        pendingRightTranslations[instanceId] = __instance;
                        MelonLogger.Msg($"Right? -> PENDING (unknown context, ID: {instanceId})");
                        
                        currentTexts[instanceId] = value;
                        return;
                    }
                }

                if (value == "Are you okay?")
                {
                    MelonLogger.Msg($"=== ARE YOU OKAY? DETECTED ===");
                    MelonLogger.Msg($"Last dialogue (original): '{lastLongDialogueOriginal}'");
                    
                    string context = GetAreYouOkayContext();
                    MelonLogger.Msg($"Are you okay? context: {context}");
                    
                    if (context == "MASSAGE")
                    {
                        SetTranslatedText(__instance, "Все хорошо?", instanceId);
                        MelonLogger.Msg("Are you okay? -> Все хорошо? (massage context)");
                    }
                    else if (context == "PERFORMANCE_STRESS")
                    {
                        SetTranslatedText(__instance, "Все в порядке?", instanceId);
                        MelonLogger.Msg("Are you okay? -> Все в порядке? (performance stress context)");
                    }
                    else
                    {
                        SetTranslatedText(__instance, "С тобой все в порядке?", instanceId);
                        MelonLogger.Msg("Are you okay? -> С тобой все в порядке? (default context)");
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
                        MelonLogger.Error($"Error in TMP setText: {e}");
                        isSettingText = false;
                    }
                };
                
                ProcessText(__instance.gameObject, value, instanceId, setText);
            }
            catch (Exception e)
            {
                MelonLogger.Error($"Error in TranslateTMPText: {e}");
            }
        }
    }

    private static void HandleColorText(TMP_Text component, string colorValue)
    {
        lock (translateLock)
        {
            int instanceId = component.GetInstanceID();
            
            MelonLogger.Msg($"=== COLOR DETECTED: {colorValue} ===");
            MelonLogger.Msg($"Current color context: {colorContext}");

            if (!string.IsNullOrEmpty(colorContext) && wasContextUsed)
            {
                double timeSinceContextUsed = (System.DateTime.Now - lastContextUsedTime).TotalSeconds;
                if (timeSinceContextUsed > 10.0f)
                {
                    MelonLogger.Msg($"[STALE CONTEXT] Context '{colorContext}' active for {timeSinceContextUsed:F2}s, resetting");
                    ResetColorContext();
                }
            }
            
            if (colorContext == "socks" && processedColorsInSeries >= 3)
            {
                MelonLogger.Msg($"[CONTEXT ERROR] socks context still active after processing {processedColorsInSeries} colors, resetting");
                ResetColorContext();
            }
            
            if (string.IsNullOrEmpty(colorContext))
            {
                string translation = GetColorTranslation(colorValue, "");
                MelonLogger.Msg($"{colorValue} -> {translation} (default, no context)");
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
        MelonLogger.Msg($"{colorValue} -> {translation} (swimsuit context)");
        
        SetTranslatedText(component, translation, instanceId);
        
        processedColorsInSeries++;
        MelonLogger.Msg($"[SWIMSUIT] Processed {processedColorsInSeries}/4 colors");
        
        if (processedColorsInSeries >= 4)
        {
            MelonLogger.Msg($"[CONTEXT COMPLETE] All {processedColorsInSeries} swimsuit colors processed");
            ResetColorContext();
        }
    }

    private static void ProcessUndiesColor(TMP_Text component, string colorValue, int instanceId)
    {
        string translation = GetColorTranslation(colorValue, "undies");
        MelonLogger.Msg($"{colorValue} -> {translation} (undies context)");
        
        SetTranslatedText(component, translation, instanceId);
        
        processedColorsInSeries++;
        MelonLogger.Msg($"[UNDIES] Processed {processedColorsInSeries}/3 colors");
        
        if (processedColorsInSeries >= 6)
        {
            MelonLogger.Msg($"[CONTEXT COMPLETE] All {processedColorsInSeries} undies colors processed");
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
                
                MelonLogger.Msg($"[SetTranslatedText] '{originalText}' -> '{text}'");
                
                isSettingText = false;
            }
            catch (Exception e)
            {
                MelonLogger.Error($"Error in SetTranslatedText: {e}");
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
                MelonLogger.Msg($"[COLOR CONTEXT SET] Set to: swimsuit (from: '{translatedText}')");
                ProcessPendingColorsForContext();
            }
            else if (translatedText.Contains("трусики") || translatedText.Contains("они?") || 
                    translatedText.Contains("What color was it?!") ||
                    translatedText.Contains("Твои трусики") || translatedText.Contains("Your undies") ||
                    translatedText.Contains("Когда тебе станет лучше"))
            {
                colorContext = "undies";
                MelonLogger.Msg($"[COLOR CONTEXT SET] Set to: undies (from: '{translatedText}')");
                ProcessPendingColorsForContext();
            }
            else if (translatedText.Contains("Wait—what color were her socks?!") || translatedText.Contains("Ahh, so refreshing~") ||
                    translatedText.Contains("Стой — какого цвета были носки?!") || translatedText.Contains("Ахх, как освежает~"))
            {
                colorContext = "socks";
                MelonLogger.Msg($"[COLOR CONTEXT SET] Set to: socks (from: '{translatedText}')");
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
                    MelonLogger.Error("setText is null in ProcessText!");
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
                    MelonLogger.Msg($"PROCESS TEXT: '{value}' -> '{touchTranslated}'");
                    return;
                }
                
                if (!string.IsNullOrEmpty(value) && value.Length >= 15 && !IsSystemText(value) && !value.Contains("UnityExplorer"))
                {
                    lastLongDialogue = value;
                    lastLongDialogueOriginal = value;
                    MelonLogger.Msg($"[LAST DIALOGUE] Set to: '{value}'");
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
                    MelonLogger.Msg($"TRANSLATED: '{value}' -> '{translated}'");
                }
                else
                {
                    setText(value);
                    currentTexts[instanceId] = value;
                }
            }
            catch (Exception e)
            {
                MelonLogger.Error($"Error in ProcessText: {e}");
                try
                {
                    if (setText != null)
                        setText(value);
                }
                catch (Exception ex)
                {
                    MelonLogger.Error($"Error in fallback setText: {ex}");
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