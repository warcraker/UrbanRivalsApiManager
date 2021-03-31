using System;
using System.Collections.Generic;

namespace Warcraker.UrbanRivals.ApiManager
{
    /// <summary>
    /// <para>List of all Urban Rivals API calls. Deprecated calls not included.</para>
    /// <para>Check the official documentation BEFORE using any of these calls: <seealso href="http://www.urban-rivals.com/api/developer/"/></para>
    /// </summary>
    public class ApiCallList
    {
        public class Characters
        {
            public class GetCharacterLevels : ApiCall
            {
                private GetCharacterLevels()
                    : base("characters.getCharacterLevels")
                {
                    RequiresPublicAccess = true;
                    ReturnsContext = true;
                    ReturnsItems = true;

                    AddCompulsoryParameter("characterID");
                    AddParameter("levelMax");
                    AddParameter("imageSize");
                    AddParameter("imageFormat");
                }
                public GetCharacterLevels(int characterID)
                    : this()
                {
                    SetParamenterValue("characterID", characterID);
                }
                public int characterID 
                { 
                    get { return (int)GetParameterValue("characterID"); } 
                    set { SetParamenterValue("characterID", value); } 
                }
                public int levelMax 
                {
                    get { return (int)GetParameterValue("levelMax"); }
                    set { SetParamenterValue("levelMax", value); }
                }
                public string imageSize
                {
                    get { return (string)GetParameterValue("imageSize"); }
                    set { SetParamenterValue("imageSize", value); }
                }
                public string imageFormat
                {
                    get { return (string)GetParameterValue("imageFormat"); }
                    set { SetParamenterValue("imageFormat", value); }
                }
            }
            public class GetCharacters : ApiCall
            {
                public GetCharacters()
                    : base("characters.getCharacters")
                {
                    RequiresPublicAccess = true;
                    ReturnsContext = true;
                    ReturnsItems = true;

                    AddParameter("charactersIDs");
                    AddParameter("clanID");
                    AddParameter("sortby");
                    AddParameter("orderby");
                    AddParameter("imageSize");
                    AddParameter("imageFormat");
                    AddParameter("maxLevels");
                }
                public List<int> charactersIDs
                {
                    get { return (List<int>)GetParameterValue("charactersIDs"); }
                    set { SetParamenterValue("charactersIDs", value); }
                }
                public int clanID
                {
                    get { return (int)GetParameterValue("clanID"); }
                    set { SetParamenterValue("clanID", value); }
                }
                public string sortby
                {
                    get { return (string)GetParameterValue("sortby"); }
                    set { SetParamenterValue("sortby", value); }
                }
                public string orderby
                {
                    get { return (string)GetParameterValue("orderby"); }
                    set { SetParamenterValue("orderby", value); }
                }
                public string imageSize
                {
                    get { return (string)GetParameterValue("imageSize"); }
                    set { SetParamenterValue("imageSize", value); }
                }
                public string imageFormat
                {
                    get { return (string)GetParameterValue("imageFormat"); }
                    set { SetParamenterValue("imageFormat", value); }
                }
                public bool maxLevels
                {
                    get { return (bool)GetParameterValue("maxLevels"); }
                    set { SetParamenterValue("maxLevels", value); }
                }
            }
            public class GetClans : ApiCall
            {
                public GetClans()
                    : base("characters.getClans")
                {
                    RequiresPublicAccess = true;
                    ReturnsContext = true;
                    ReturnsItems = true;

                    AddParameter("imageSize");
                    AddParameter("imageFormat");
                }
                public string imageSize
                {
                    get { return (string)GetParameterValue("imageSize"); }
                    set { SetParamenterValue("imageSize", value); }
                }
                public string imageFormat
                {
                    get { return (string)GetParameterValue("imageFormat"); }
                    set { SetParamenterValue("imageFormat", value); }
                }
            }
            public class GetDeckFormats : ApiCall
            {
                public GetDeckFormats()
                    : base("characters.getDeckFormats")
                {
                    RequiresPublicAccess = true;
                    ReturnsContext = true;
                    ReturnsItems = true;

                    AddParameter("deckFormatID");
                    AddParameter("characterIDs");
                }
                public int deckFormatID
                {
                    get { return (int)GetParameterValue("deckFormatID"); }
                    set { SetParamenterValue("deckFormatID", value); }
                }
                public List<int> characterIDs
                {
                    get { return (List<int>)GetParameterValue("characterIDs"); }
                    set { SetParamenterValue("characterIDs", value); }
                }
            }
            public class GetPenalizedCharacters : ApiCall
            {
                public GetPenalizedCharacters()
                    : base("characters.getPenalizedCharacters")
                {
                    RequiresPublicAccess = true;
                    ReturnsContext = true;
                    ReturnsItems = true;
                }
            }
        }
        public class Collections
        {
            public class AlterCharacterDeckState : ApiCall
            {
                private AlterCharacterDeckState()
                    : base("collections.alterCharacterDeckState")
                {
                    RequiresUserAccess = true;
                    ReturnsContext = true;

                    AddCompulsoryParameter("characterInCollectionID");
                    AddParameter("newDeckState");
                }
                public AlterCharacterDeckState(int characterInCollectionID)
                    : this()
                {
                    SetParamenterValue("characterInCollectionID", characterInCollectionID);
                }
                public int characterInCollectionID
                {
                    get { return (int)GetParameterValue("characterInCollectionID"); }
                    set { SetParamenterValue("characterInCollectionID", value); }
                }
                public bool newDeckState
                {
                    get { return (bool)GetParameterValue("newDeckState"); }
                    set { SetParamenterValue("newDeckState", value); }
                }

            }
            public class CheckPresetIsStillValid : ApiCall
            {
                private CheckPresetIsStillValid()
                    : base("collections.checkPresetIsStillValid")
                {
                    RequiresUserAccess = true;
                    ReturnsContext = true;

                    AddCompulsoryParameter("presetID");
                }
                public CheckPresetIsStillValid(int presetID)
                    : this()
                {
                    SetParamenterValue("presetID", presetID);
                }
                public int presetID
                {
                    get { return (int)GetParameterValue("presetID"); }
                    set { SetParamenterValue("presetID", value); }
                }
            }
            public class DeleteSavedPreset : ApiCall
            {
                private DeleteSavedPreset()
                    : base("collections.deleteSavedPreset")
                {
                    RequiresUserAccess = true;
                    ReturnsContext = true;

                    AddCompulsoryParameter("presetID");
                }
                public DeleteSavedPreset(int presetID)
                    : this()
                {
                    SetParamenterValue("presetID", presetID);
                }
                public int presetID
                {
                    get { return (int)GetParameterValue("presetID"); }
                    set { SetParamenterValue("presetID", value); }
                }
            }
            public class GetBestCharacterVariationsWithoutEvoMax : ApiCall
            {
                public GetBestCharacterVariationsWithoutEvoMax()
                    : base("collections.getBestCharacterVariationsWithoutEvoMax")
                {
                    RequiresUserAccess = true;
                    ReturnsContext = true;
                    ReturnsItems = true;

                    AddParameter("nbItems");
                    AddParameter("imageSize");
                    AddParameter("imageFormat");
                }
                public int nbItems
                {
                    get { return (int)GetParameterValue("nbItems"); }
                    set { SetParamenterValue("nbItems", value); }
                }
                public string imageSize
                {
                    get { return (string)GetParameterValue("imageSize"); }
                    set { SetParamenterValue("imageSize", value); }
                }
                public string imageFormat
                {
                    get { return (string)GetParameterValue("imageFormat"); }
                    set { SetParamenterValue("imageFormat", value); }
                }

            }
            public class GetCharacterVariations : ApiCall
            {
                private GetCharacterVariations()
                    : base("collections.getCharacterVariations")
                {
                    RequiresUserAccess = true;
                    ReturnsContext = true;
                    ReturnsItems = true;

                    AddCompulsoryParameter("characterID");
                    AddParameter("imageSize");
                    AddParameter("imageFormat");
                }
                public GetCharacterVariations(int characterID)
                    : this ()
                {
                    SetParamenterValue("characterID", characterID);
                }
                public int characterID
                {
                    get { return (int)GetParameterValue("characterID"); }
                    set { SetParamenterValue("characterID", value); }
                }
                public string imageSize
                {
                    get { return (string)GetParameterValue("imageSize"); }
                    set { SetParamenterValue("imageSize", value); }
                }
                public string imageFormat
                {
                    get { return (string)GetParameterValue("imageFormat"); }
                    set { SetParamenterValue("imageFormat", value); }
                }

            }
            public class GetClanSummary : ApiCall
            {
                public GetClanSummary()
                    : base("collections.getClanSummary")
                {
                    RequiresUserAccess = true;
                    ReturnsContext = true;
                    ReturnsItems = true;

                    AddParameter("clanID");
                    AddParameter("imageSize");
                    AddParameter("imageFormat");
                    AddParameter("addBestCharacter");
                    AddParameter("ownedOnly");
                }
                public int clanID
                {
                    get { return (int)GetParameterValue("clanID"); }
                    set { SetParamenterValue("clanID", value); }
                }
                public string imageSize
                {
                    get { return (string)GetParameterValue("imageSize"); }
                    set { SetParamenterValue("imageSize", value); }
                }
                public string imageFormat
                {
                    get { return (string)GetParameterValue("imageFormat"); }
                    set { SetParamenterValue("imageFormat", value); }
                }
                public bool addBestCharacter
                {
                    get { return (bool)GetParameterValue("addBestCharacter"); }
                    set { SetParamenterValue("addBestCharacter", value); }
                }
                public bool ownedOnly
                {
                    get { return (bool)GetParameterValue("ownedOnly"); }
                    set { SetParamenterValue("ownedOnly", value); }
                }

            }
            public class GetCollectionPage : ApiCall
            {
                public GetCollectionPage()
                    : base("collections.getCollectionPage")
                {
                    RequiresUserAccess = true;
                    ReturnsContext = true;
                    ReturnsItems = true;

                    AddParameter("deckOnly");
                    AddParameter("page");
                    AddParameter("nbPerPage");
                    AddParameter("clanID");
                    AddParameter("groupBy");
                    AddParameter("sortBy");
                    AddParameter("orderBy");
                    AddParameter("search");
                    AddParameter("imageSize");
                    AddParameter("imageFormat");
                }
                public bool deckOnly
                {
                    get { return (bool)GetParameterValue("deckOnly"); }
                    set { SetParamenterValue("deckOnly", value); }
                }
                public int page
                {
                    get { return (int)GetParameterValue("page"); }
                    set { SetParamenterValue("page", value); }
                }
                public int nbPerPage
                {
                    get { return (int)GetParameterValue("nbPerPage"); }
                    set { SetParamenterValue("nbPerPage", value); }
                }
                public int clanID
                {
                    get { return (int)GetParameterValue("clanID"); }
                    set { SetParamenterValue("clanID", value); }
                }
                public string groupBy
                {
                    get { return (string)GetParameterValue("groupBy"); }
                    set { SetParamenterValue("groupBy", value); }
                }
                public string sortBy
                {
                    get { return (string)GetParameterValue("sortBy"); }
                    set { SetParamenterValue("sortBy", value); }
                }
                public string orderBy
                {
                    get { return (string)GetParameterValue("orderBy"); }
                    set { SetParamenterValue("orderBy", value); }
                }
                public string search
                {
                    get { return (string)GetParameterValue("search"); }
                    set { SetParamenterValue("search", value); }
                }
                public string imageSize
                {
                    get { return (string)GetParameterValue("imageSize"); }
                    set { SetParamenterValue("imageSize", value); }
                }
                public string imageFormat
                {
                    get { return (string)GetParameterValue("imageFormat"); }
                    set { SetParamenterValue("imageFormat", value); }
                }
            }
            public class GetDeck : ApiCall
            {
                public GetDeck()
                    : base("collections.getDeck")
                {
                    RequiresUserAccess = true;
                    ReturnsContext = true;
                    ReturnsItems = true;

                    AddParameter("imageSize");
                    AddParameter("imageFormat");
                    AddParameter("presetID");
                }
                public string imageSize
                {
                    get { return (string)GetParameterValue("imageSize"); }
                    set { SetParamenterValue("imageSize", value); }
                }
                public string imageFormat
                {
                    get { return (string)GetParameterValue("imageFormat"); }
                    set { SetParamenterValue("imageFormat", value); }
                }
                public int presetID
                {
                    get { return (int)GetParameterValue("presetID"); }
                    set { SetParamenterValue("presetID", value); }
                }

            }
            public class GetPresets : ApiCall
            {
                public GetPresets()
                    : base("collections.getPresets")
                {
                    RequiresUserAccess = true;
                    ReturnsContext = true;
                    ReturnsItems = true;
                }
            }
            public class GetSelectionFormatsCompatibility : ApiCall
            {
                public GetSelectionFormatsCompatibility()
                    : base("collections.getSelectionFormatsCompatibility")
                {
                    RequiresUserAccess = true;
                    ReturnsContext = true;
                    ReturnsItems = true;

                    AddParameter("characterInCollectionsIDs");
                }
                public List<int> characterInCollectionsIDs
                {
                    get { return (List<int>)GetParameterValue("characterInCollectionsIDs"); }
                    set { SetParamenterValue("characterInCollectionsIDs", value); }
                }
            }
            public class GetSummary : ApiCall
            {
                public GetSummary()
                    : base("collections.getSummary")
                {
                    RequiresUserAccess = true;
                    ReturnsContext = true;
                    ReturnsItems = true;

                    AddParameter("imageSize");
                    AddParameter("imageFormat");
                }
                public string imageSize
                {
                    get { return (string)GetParameterValue("imageSize"); }
                    set { SetParamenterValue("imageSize", value); }
                }
                public string imageFormat
                {
                    get { return (string)GetParameterValue("imageFormat"); }
                    set { SetParamenterValue("imageFormat", value); }
                }
            }
            public class GiveXPFromReserve : ApiCall
            {
                private GiveXPFromReserve()
                    : base("collections.giveXPFromReserve")
                {
                    RequiresActionAccess = true;
                    ReturnsContext = true;

                    AddCompulsoryParameter("characterInCollectionID");
                    AddParameter("battleID");
                    AddParameter("imageSize");
                    AddParameter("imageFormat");
                }
                public GiveXPFromReserve(int characterInCollectionID)
                    : this()
                {
                    SetParamenterValue("characterInCollectionID", characterInCollectionID);
                }
                public int characterInCollectionID
                {
                    get { return (int)GetParameterValue("characterInCollectionID"); }
                    set { SetParamenterValue("characterInCollectionID", value); }
                }
                public int battleID
                {
                    get { return (int)GetParameterValue("battleID"); }
                    set { SetParamenterValue("battleID", value); }
                }
                public string imageSize
                {
                    get { return (string)GetParameterValue("imageSize"); }
                    set { SetParamenterValue("imageSize", value); }
                }
                public string imageFormat
                {
                    get { return (string)GetParameterValue("imageFormat"); }
                    set { SetParamenterValue("imageFormat", value); }
                }
            }
            public class LoadSavedPreset : ApiCall
            {
                public LoadSavedPreset()
                    : base ("collections.loadSavedPreset")
                {
                    RequiresUserAccess = true;
                    ReturnsContext = true;
                }
                public int presetID
                {
                    get { return (int)GetParameterValue("presetID"); }
                    set { SetParamenterValue("presetID", value); }
                }
            }          
            public class SaveDeckAsPreset : ApiCall
            {
                private SaveDeckAsPreset()
                    : base("collections.saveDeckAsPreset")
                {
                    RequiresUserAccess = true;
                    ReturnsContext = true;

                    AddCompulsoryParameter("presetID");
                    AddCompulsoryParameter("presetName");
                }
                public SaveDeckAsPreset(int presetID, string presetName)
                    : this()
                {
                    SetParamenterValue("presetID", presetID);
                    SetParamenterValue("presetName", presetName);
                }
                public int presetID
                {
                    get { return (int)GetParameterValue("presetID"); }
                    set { SetParamenterValue("presetID", value); }
                }
                public string presetName
                {
                    get { return (string)GetParameterValue("presetName"); }
                    set { SetParamenterValue("presetName", value); }
                }
            }
            public class SaveSelectionAsPreset : ApiCall
            {
                public SaveSelectionAsPreset()
                    : base("collections.saveSelectionAsPreset")
                {
                    RequiresUserAccess = true;
                    ReturnsContext = true;

                    AddParameter("characterInCollectionsIDs");
                    AddParameter("presetID");
                    AddParameter("presetName");
                }
                public List<int> characterInCollectionsIDs
                {
                    get { return (List<int>)GetParameterValue("characterInCollectionsIDs"); }
                    set { SetParamenterValue("characterInCollectionsIDs", value); }
                }
                public int presetID
                {
                    get { return (int)GetParameterValue("presetID"); }
                    set { SetParamenterValue("presetID", value); }
                }
                public string presetName
                {
                    get { return (string)GetParameterValue("presetName"); }
                    set { SetParamenterValue("presetName", value); }
                }
            }
            public class Search : ApiCall
            {
                private Search()
                    : base("collections.search")
                {
                    RequiresUserAccess = true;
                    ReturnsContext = true;
                    ReturnsItems = true;

                    AddCompulsoryParameter("searchText");
                    AddParameter("imageSize");
                    AddParameter("imageFormat");
                }
                public Search(string searchText)
                    : this()
                {
                    SetParamenterValue("searchText", searchText);
                }
                public string searchText
                {
                    get { return (string)GetParameterValue("searchText"); }
                    set { SetParamenterValue("searchText", value); }
                }
                public string imageSize
                {
                    get { return (string)GetParameterValue("imageSize"); }
                    set { SetParamenterValue("imageSize", value); }
                }
                public string imageFormat
                {
                    get { return (string)GetParameterValue("imageFormat"); }
                    set { SetParamenterValue("imageFormat", value); }
                }
            }
            public class SetSelectionAsDeck : ApiCall
            {
                public SetSelectionAsDeck()
                    : base("collections.setSelectionAsDeck")
                {
                    RequiresUserAccess = true;
                    ReturnsContext = true;

                    AddParameter("characterInCollectionIDs");
                }
                public List<int> characterInCollectionIDs
                {
                    get { return (List<int>)GetParameterValue("characterInCollectionIDs"); }
                    set { SetParamenterValue("characterInCollectionIDs", value); }
                }
            }
        }
        public class Forums
        {
            public class CreateMessage : ApiCall
            {
                private CreateMessage()
                    : base("forums.createMessage")
                {
                    RequiresUserAccess = true;
                    ReturnsContext = true;

                    AddCompulsoryParameter("subjectID");
                    AddCompulsoryParameter("message");
                }
                public CreateMessage(int subjectID, string message)
                    : this()
                {
                    SetParamenterValue("subjectID", subjectID);
                    SetParamenterValue("message", message);
                }
                public int subjectID
                {
                    get { return (int)GetParameterValue("subjectID"); }
                    set { SetParamenterValue("subjectID", value); }
                }
                public string message
                {
                    get { return (string)GetParameterValue("message"); }
                    set { SetParamenterValue("message", value); }
                }
            }
            public class CreateSubject : ApiCall
            {
                private CreateSubject()
                    : base("forums.createSubject")
                {
                    RequiresUserAccess = true;
                    ReturnsContext = true;

                    AddCompulsoryParameter("themeID");
                    AddCompulsoryParameter("title");
                    AddCompulsoryParameter("message");
                }
                public CreateSubject(int themeID, string title, string message)
                    : this()
                {
                    SetParamenterValue("themeID", themeID);
                    SetParamenterValue("title", title);
                    SetParamenterValue("message", message);
                }
                public int themeID
                {
                    get { return (int)GetParameterValue("themeID"); }
                    set { SetParamenterValue("themeID", value); }
                }
                public string title
                {
                    get { return (string)GetParameterValue("title"); }
                    set { SetParamenterValue("title", value); }
                }
                public string message
                {
                    get { return (string)GetParameterValue("message"); }
                    set { SetParamenterValue("message", value); }
                }
            }
            public class GetMessagesPage : ApiCall
            {
                private GetMessagesPage()
                    : base("forums.getMessagesPage")
                {
                    RequiresPublicAccess = true;
                    ReturnsContext = true;
                    ReturnsItems = true;

                    AddCompulsoryParameter("subjectID");
                    AddParameter("page");
                    AddParameter("nbPerPage");
                }
                public GetMessagesPage(int subjectID)
                    : this()
                {
                    SetParamenterValue("subjectID", subjectID);
                }
                public int subjectID
                {
                    get { return (int)GetParameterValue("subjectID"); }
                    set { SetParamenterValue("subjectID", value); }
                }
                public int page
                {
                    get { return (int)GetParameterValue("page"); }
                    set { SetParamenterValue("page", value); }
                }
                public int nbPerPage
                {
                    get { return (int)GetParameterValue("nbPerPage"); }
                    set { SetParamenterValue("nbPerPage", value); }
                }
            }
            public class GetSubjectsPage : ApiCall
            {
                private GetSubjectsPage()
                    : base("forums.getSubjectsPage")
                {
                    RequiresPublicAccess = true;
                    ReturnsContext = true;
                    ReturnsItems = true;

                    AddCompulsoryParameter("themeID");
                    AddParameter("page");
                    AddParameter("nbPerPage");
                }
                public GetSubjectsPage(int themeID)
                    : this()
                {
                    SetParamenterValue("themeID", themeID);
                }
                public int themeID
                {
                    get { return (int)GetParameterValue("themeID"); }
                    set { SetParamenterValue("themeID", value); }
                }
                public int page
                {
                    get { return (int)GetParameterValue("page"); }
                    set { SetParamenterValue("page", value); }
                }
                public int nbPerPage
                {
                    get { return (int)GetParameterValue("nbPerPage"); }
                    set { SetParamenterValue("nbPerPage", value); }
                }
            }
            public class GetThemes : ApiCall
            {
                public GetThemes()
                    : base("forums.getThemes")
                {
                    RequiresPublicAccess = true;
                    ReturnsContext = true;
                    ReturnsItems = true;
                }
            }
        }
        public class General
        {
            public class FindPlayersByName : ApiCall
            {
                private FindPlayersByName()
                    : base("general.findPlayersByName")
                {
                    RequiresPublicAccess = true;
                    ReturnsContext = true;
                    ReturnsItems = true;

                    AddCompulsoryParameter("partOfName");
                }
                public FindPlayersByName(string partOfName)
                    : this()
                {
                    SetParamenterValue("partOfName", partOfName);
                }
                public string partOfName
                {
                    get { return (string)GetParameterValue("partOfName"); }
                    set { SetParamenterValue("partOfName", value); }
                }
            }
            public class GetCountries : ApiCall
            {
                public GetCountries()
                    : base("general.getCountries")
                {
                    RequiresPublicAccess = true;
                    ReturnsContext = true;
                    ReturnsItems = true;
                }
            }
            public class GetDailyReward : ApiCall
            {
                public GetDailyReward()
                    : base("general.getDailyReward ")
                {
                    RequiresUserAccess = true;
                    ReturnsContext = true;
                    ReturnsItems = true;

                    AddParameter("notified");
                }
                public int notified
                {
                    get { return (int)GetParameterValue("notified"); }
                    set { SetParamenterValue("notified", value); }
                }
            }
            public class GetNewCharactersAdParams : ApiCall
            {
                public GetNewCharactersAdParams()
                    : base("general.getNewCharactersAdParams")
                {
                    RequiresPublicAccess = true;
                    ReturnsContext = true;
                }
            }
            public class GetNews : ApiCall
            {
                public GetNews()
                    : base("general.getNews")
                {
                    RequiresPublicAccess = true;
                    ReturnsContext = true;
                    ReturnsItems = true;

                    AddParameter("nbNews");
                    AddParameter("type");
                }
                public int nbNews
                {
                    get { return (int)GetParameterValue("nbNews"); }
                    set { SetParamenterValue("nbNews", value); }
                }
                public string type
                {
                    get { return (string)GetParameterValue("type"); }
                    set { SetParamenterValue("type", value); }
                }
            }
            public class GetPlayer : ApiCall
            {
                public GetPlayer()
                    : base("general.getPlayer")
                {
                    RequiresUserAccess = true;
                    ReturnsContext = true;

                    AddParameter("imageSize");
                    AddParameter("imageFormat");
                }
                public string imageSize
                {
                    get { return (string)GetParameterValue("imageSize"); }
                    set { SetParamenterValue("imageSize", value); }
                }
                public string imageFormat
                {
                    get { return (string)GetParameterValue("imageFormat"); }
                    set { SetParamenterValue("imageFormat", value); }
                }
            }
            public class GetPlayersPoints : ApiCall
            {
                public GetPlayersPoints()
                    : base("general.getPlayersPoints")
                {
                    RequiresPublicAccess = true;
                    ReturnsContext = true;

                    AddParameter("playerIDs");
                }
                public List<int> playerIDs
                {
                    get { return (List<int>)GetParameterValue("playerIDs"); }
                    set { SetParamenterValue("playerIDs", value); }
                }
            }
            public class GetSpecialEventStatus : ApiCall
            {
                public GetSpecialEventStatus()
                    : base("general.getSpecialEventStatus")
                {
                    RequiresPublicAccess = true;
                    ReturnsContext = true;
                    ReturnsItems = true;
                }
            }
            public class GetSupportedLanguages : ApiCall
            {
                public GetSupportedLanguages()
                    : base("general.getSupportedLanguages")
                {
                    RequiresPublicAccess = true;
                    ReturnsContext = true;
                    ReturnsItems = true;
                }
            }
            public class GetTips : ApiCall
            {
                public GetTips()
                    : base("general.getTips")
                {
                    RequiresPublicAccess = true;
                    ReturnsContext = true;
                    ReturnsItems = true;
                }
            }
            public class GetTournamentsPeriods : ApiCall
            {
                public GetTournamentsPeriods()
                    : base("general.getTournamentsPeriods")
                {
                    RequiresPublicAccess = true;
                    ReturnsContext = true;
                }
            }
            public class RemovePresence : ApiCall
            {
                public RemovePresence()
                    : base("general.removePresence")
                {
                    RequiresUserAccess = true;
                    ReturnsContext = true;
                }
            }
            public class SetPresenceVisibility : ApiCall
            {
                private SetPresenceVisibility()
                    : base("general.setPresenceVisibility")
                {
                    RequiresUserAccess = true;
                    ReturnsContext = true;

                    AddCompulsoryParameter("visible");
                }
                public SetPresenceVisibility(string visible)
                    : this()
                {
                    SetParamenterValue("visible", visible);
                }
                public string visible
                {
                    get { return (string)GetParameterValue("visible"); }
                    set { SetParamenterValue("visible", value); }
                }
            }
            public class UpdatePresence : ApiCall
            {
                public UpdatePresence()
                    : base("general.updatePresence")
                {
                    RequiresUserAccess = true;
                    ReturnsContext = true;

                    AddParameter("apiConsumerMedium");
                    AddParameter("apiConsumerID");
                    AddParameter("apiConsumerExtraInfos");
                }
                public string apiConsumerMedium
                {
                    get { return (string)GetParameterValue("apiConsumerMedium"); }
                    set { SetParamenterValue("apiConsumerMedium", value); }
                }
                public string apiConsumerID
                {
                    get { return (string)GetParameterValue("apiConsumerID"); }
                    set { SetParamenterValue("apiConsumerID", value); }
                }
                public string apiConsumerExtraInfos
                {
                    get { return (string)GetParameterValue("apiConsumerExtraInfos"); }
                    set { SetParamenterValue("apiConsumerExtraInfos", value); }
                }
            }
        }
        public class Guilds
        {
            public class GetGuild : ApiCall
            {
                public GetGuild()
                    : base("guilds.getGuild")
                {
                    RequiresPublicAccess = true;
                    ReturnsContext = true;
                    ReturnsItems = true;

                    AddParameter("guildID");
                    AddParameter("imageSize");
                    AddParameter("imageFormat");
                }
                public int guildID
                {
                    get { return (int)GetParameterValue("guildID"); }
                    set { SetParamenterValue("guildID", value); }
                }
                public string imageSize
                {
                    get { return (string)GetParameterValue("imageSize"); }
                    set { SetParamenterValue("imageSize", value); }
                }
                public string imageFormat
                {
                    get { return (string)GetParameterValue("imageFormat"); }
                    set { SetParamenterValue("imageFormat", value); }
                }
            }
            public class GetGuildsPage : ApiCall
            {
                public GetGuildsPage()
                    : base("guilds.getGuildsPage")
                {
                    RequiresPublicAccess = true;
                    ReturnsContext = true;
                    ReturnsItems = true;

                    AddParameter("page");
                    AddParameter("nbPerPage");
                    AddParameter("country");
                    AddParameter("recruiting");
                }
                public int page
                {
                    get { return (int)GetParameterValue("page"); }
                    set { SetParamenterValue("page", value); }
                }
                public int nbPerPage
                {
                    get { return (int)GetParameterValue("nbPerPage"); }
                    set { SetParamenterValue("nbPerPage", value); }
                }
                public string country
                {
                    get { return (string)GetParameterValue("country"); }
                    set { SetParamenterValue("country", value); }
                }
                public bool recruiting
                {
                    get { return (bool)GetParameterValue("recruiting"); }
                    set { SetParamenterValue("recruiting", value); }
                }
            }
            public class JoinGuild : ApiCall
            {
                private JoinGuild()
                    : base("guilds.joinGuild")
                {
                    RequiresActionAccess = true;
                    ReturnsContext = true;

                    AddCompulsoryParameter("guildID");
                }

                public JoinGuild(int guildID)
                    : this()
                {
                    SetParamenterValue("guildID", guildID);
                }
                public int guildID
                {
                    get { return (int)GetParameterValue("guildID"); }
                    set { SetParamenterValue("guildID", value); }
                }
            }
            public class LeaveGuild : ApiCall
            {
                public LeaveGuild()
                    : base("guilds.leaveGuild")
                {
                    RequiresActionAccess = true;
                    ReturnsContext = true;
                }
            }
            public class SendGuildMsg : ApiCall
            {
                private SendGuildMsg()
                    : base("guilds.sendGuildMsg")
                {
                    RequiresActionAccess = true;
                    ReturnsContext = true;

                    AddCompulsoryParameter("msg");
                }
                public SendGuildMsg(string msg)
                    : this()
                {
                    SetParamenterValue("msg", msg);
                }
                public string msg
                {
                    get { return (string)GetParameterValue("msg"); }
                    set { SetParamenterValue("msg", value); }
                }
            }
            public class SwitchGuildMsgSetting : ApiCall
            {
                public SwitchGuildMsgSetting()
                    : base("guilds.switchGuildMsgSetting")
                {
                    RequiresActionAccess = true;
                    ReturnsContext = true;
                }
            }
        }
        public class Market
        {
            public class CancelSale : ApiCall
            {
                private CancelSale()
                    : base("market.cancelSale")
                {
                    RequiresActionAccess = true;
                    ReturnsContext = true;

                    AddCompulsoryParameter("characterInMarketID");
                }
                public CancelSale(int characterInMarketID)
                    : this()
                {
                    SetParamenterValue("characterInMarketID", characterInMarketID);
                }
                public int characterInMarketID
                {
                    get { return (int)GetParameterValue("characterInMarketID"); }
                    set { SetParamenterValue("characterInMarketID", value); }
                }
            }
            public class GetCharactersPricesCurrent : ApiCall
            {
                private GetCharactersPricesCurrent()
                    : base("market.getCharactersPricesCurrent")
                {
                    RequiresPublicAccess = true;
                    ReturnsContext = true;

                    AddCompulsoryParameter("charactersIDs");
                    AddParameter("charactersLevels");
                    AddParameter("clanID");
                }
                public GetCharactersPricesCurrent(List<int> charactersIDs)
                    : this()
                {
                    SetParamenterValue("charactersIDs", charactersIDs);
                }
                public List<int> charactersIDs
                {
                    get { return (List<int>)GetParameterValue("charactersIDs"); }
                    set { SetParamenterValue("charactersIDs", value); }
                }
                public List<int> charactersLevels
                {
                    get { return (List<int>)GetParameterValue("charactersLevels"); }
                    set { SetParamenterValue("charactersLevels", value); }
                }
                public int clanID
                {
                    get { return (int)GetParameterValue("clanID"); }
                    set { SetParamenterValue("clanID", value); }
                }
            }
            public class GetCurrentSales : ApiCall
            {
                public GetCurrentSales()
                    : base("market.getCurrentSales")
                {
                    RequiresUserAccess = true;
                    ReturnsContext = true;
                    ReturnsItems = true;

                    AddParameter("page");
                    AddParameter("nbPerPage");
                    AddParameter("clanID");
                    AddParameter("groupType");
                    AddParameter("sortBy");
                    AddParameter("orderBy");
                    AddParameter("imageSize");
                    AddParameter("imageFormat");
                }
                public int page
                {
                    get { return (int)GetParameterValue("page"); }
                    set { SetParamenterValue("page", value); }
                }
                public int nbPerPage
                {
                    get { return (int)GetParameterValue("nbPerPage"); }
                    set { SetParamenterValue("nbPerPage", value); }
                }
                public int clanID
                {
                    get { return (int)GetParameterValue("clanID"); }
                    set { SetParamenterValue("clanID", value); }
                }
                public string groupType
                {
                    get { return (string)GetParameterValue("groupType"); }
                    set { SetParamenterValue("groupType", value); }
                }
                public string sortBy
                {
                    get { return (string)GetParameterValue("sortBy"); }
                    set { SetParamenterValue("sortBy", value); }
                }
                public string orderBy
                {
                    get { return (string)GetParameterValue("orderBy"); }
                    set { SetParamenterValue("orderBy", value); }
                }
                public string imageSize
                {
                    get { return (string)GetParameterValue("imageSize"); }
                    set { SetParamenterValue("imageSize", value); }
                }
                public string imageFormat
                {
                    get { return (string)GetParameterValue("imageFormat"); }
                    set { SetParamenterValue("imageFormat", value); }
                }
            }
            public class GetHistoryPurchases : ApiCall
            {
                public GetHistoryPurchases()
                    : base("market.getHistoryPurchases")
                {
                    RequiresUserAccess = true;
                    ReturnsContext = true;
                    ReturnsItems = true;

                    AddParameter("page");
                    AddParameter("nbPerPage");
                    AddParameter("clanID");
                    AddParameter("sortBy");
                    AddParameter("orderBy");
                    AddParameter("imageSize");
                    AddParameter("imageFormat");
                }
                public int page
                {
                    get { return (int)GetParameterValue("page"); }
                    set { SetParamenterValue("page", value); }
                }
                public int nbPerPage
                {
                    get { return (int)GetParameterValue("nbPerPage"); }
                    set { SetParamenterValue("nbPerPage", value); }
                }
                public int clanID
                {
                    get { return (int)GetParameterValue("clanID"); }
                    set { SetParamenterValue("clanID", value); }
                }
                public string sortBy
                {
                    get { return (string)GetParameterValue("sortBy"); }
                    set { SetParamenterValue("sortBy", value); }
                }
                public string orderBy
                {
                    get { return (string)GetParameterValue("orderBy"); }
                    set { SetParamenterValue("orderBy", value); }
                }
                public string imageSize
                {
                    get { return (string)GetParameterValue("imageSize"); }
                    set { SetParamenterValue("imageSize", value); }
                }
                public string imageFormat
                {
                    get { return (string)GetParameterValue("imageFormat"); }
                    set { SetParamenterValue("imageFormat", value); }
                }
            }
            public class GetHistorySales : ApiCall
            {
                public GetHistorySales()
                    : base("market.getHistorySales")
                {
                    RequiresUserAccess = true;
                    ReturnsContext = true;
                    ReturnsItems = true;

                    AddParameter("page");
                    AddParameter("nbPerPage");
                    AddParameter("clanID");
                    AddParameter("sortBy");
                    AddParameter("orderBy");
                    AddParameter("imageSize");
                    AddParameter("imageFormat");
                }
                public int page
                {
                    get { return (int)GetParameterValue("page"); }
                    set { SetParamenterValue("page", value); }
                }
                public int nbPerPage
                {
                    get { return (int)GetParameterValue("nbPerPage"); }
                    set { SetParamenterValue("nbPerPage", value); }
                }
                public int clanID
                {
                    get { return (int)GetParameterValue("clanID"); }
                    set { SetParamenterValue("clanID", value); }
                }
                public string sortBy
                {
                    get { return (string)GetParameterValue("sortBy"); }
                    set { SetParamenterValue("sortBy", value); }
                }
                public string orderBy
                {
                    get { return (string)GetParameterValue("orderBy"); }
                    set { SetParamenterValue("orderBy", value); }
                }
                public string imageSize
                {
                    get { return (string)GetParameterValue("imageSize"); }
                    set { SetParamenterValue("imageSize", value); }
                }
                public string imageFormat
                {
                    get { return (string)GetParameterValue("imageFormat"); }
                    set { SetParamenterValue("imageFormat", value); }
                }
            }
            public class GetMarketPage : ApiCall
            {
                public GetMarketPage()
                    : base("market.getMarketPage")
                {
                    RequiresPublicAccess = true;
                    ReturnsContext = true;
                    ReturnsItems = true;

                    AddParameter("page");
                    AddParameter("nbPerPage");
                    AddParameter("clanID");
                    AddParameter("groupType");
                    AddParameter("sortBy");
                    AddParameter("orderBy");
                    AddParameter("filter");
                    AddParameter("imageSize");
                    AddParameter("imageFormat");
                }
                public int page
                {
                    get { return (int)GetParameterValue("page"); }
                    set { SetParamenterValue("page", value); }
                }
                public int nbPerPage
                {
                    get { return (int)GetParameterValue("nbPerPage"); }
                    set { SetParamenterValue("nbPerPage", value); }
                }
                public int clanID
                {
                    get { return (int)GetParameterValue("clanID"); }
                    set { SetParamenterValue("clanID", value); }
                }
                public string groupType
                {
                    get { return (string)GetParameterValue("groupType"); }
                    set { SetParamenterValue("groupType", value); }
                }
                public string sortBy
                {
                    get { return (string)GetParameterValue("sortBy"); }
                    set { SetParamenterValue("sortBy", value); }
                }
                public string orderBy
                {
                    get { return (string)GetParameterValue("orderBy"); }
                    set { SetParamenterValue("orderBy", value); }
                }
                public string filter
                {
                    get { return (string)GetParameterValue("filter"); }
                    set { SetParamenterValue("filter", value); }
                }
                public string imageSize
                {
                    get { return (string)GetParameterValue("imageSize"); }
                    set { SetParamenterValue("imageSize", value); }
                }
                public string imageFormat
                {
                    get { return (string)GetParameterValue("imageFormat"); }
                    set { SetParamenterValue("imageFormat", value); }
                }
            }
            public class GetOffersForCharacter : ApiCall
            {
                private GetOffersForCharacter()
                    : base("market.getOffersForCharacter")
                {
                    RequiresPublicAccess = true;
                    ReturnsContext = true;
                    ReturnsItems = true;

                    AddCompulsoryParameter("characterID");
                    AddParameter("onlyAtLevel");
                    AddParameter("maxResults");
                    AddParameter("imageSize");
                    AddParameter("imageFormat");
                }
                public GetOffersForCharacter(int characterID)
                    : this()
                {
                    SetParamenterValue("characterID", characterID);
                }
                public int characterID
                {
                    get { return (int)GetParameterValue("characterID"); }
                    set { SetParamenterValue("characterID", value); }
                }
                public int onlyAtLevel
                {
                    get { return (int)GetParameterValue("onlyAtLevel"); }
                    set { SetParamenterValue("onlyAtLevel", value); }
                }
                public int maxResults
                {
                    get { return (int)GetParameterValue("maxResults"); }
                    set { SetParamenterValue("maxResults", value); }
                }
                public string imageSize
                {
                    get { return (string)GetParameterValue("imageSize"); }
                    set { SetParamenterValue("imageSize", value); }
                }
                public string imageFormat
                {
                    get { return (string)GetParameterValue("imageFormat"); }
                    set { SetParamenterValue("imageFormat", value); }
                }
            }
            public class GetWaitingPrivateSales : ApiCall
            {
                public GetWaitingPrivateSales()
                    : base("market.getWaitingPrivateSales")
                {
                    RequiresUserAccess = true;
                    ReturnsContext = true;
                    ReturnsItems = true;

                    AddParameter("page");
                    AddParameter("nbPerPage");
                    AddParameter("clanID");
                    AddParameter("groupType");
                    AddParameter("sortBy");
                    AddParameter("orderBy");
                    AddParameter("imageSize");
                    AddParameter("imageFormat");
                }
                public int page
                {
                    get { return (int)GetParameterValue("page"); }
                    set { SetParamenterValue("page", value); }
                }
                public int nbPerPage
                {
                    get { return (int)GetParameterValue("nbPerPage"); }
                    set { SetParamenterValue("nbPerPage", value); }
                }
                public int clanID
                {
                    get { return (int)GetParameterValue("clanID"); }
                    set { SetParamenterValue("clanID", value); }
                }
                public string groupType
                {
                    get { return (string)GetParameterValue("groupType"); }
                    set { SetParamenterValue("groupType", value); }
                }
                public string sortBy
                {
                    get { return (string)GetParameterValue("sortBy"); }
                    set { SetParamenterValue("sortBy", value); }
                }
                public string orderBy
                {
                    get { return (string)GetParameterValue("orderBy"); }
                    set { SetParamenterValue("orderBy", value); }
                }
                public string imageSize
                {
                    get { return (string)GetParameterValue("imageSize"); }
                    set { SetParamenterValue("imageSize", value); }
                }
                public string imageFormat
                {
                    get { return (string)GetParameterValue("imageFormat"); }
                    set { SetParamenterValue("imageFormat", value); }
                }
            }
            public class PurchaseOffer : ApiCall
            {
                private PurchaseOffer()
                    : base("market.purchaseOffer")
                {
                    RequiresActionAccess = true;
                    ReturnsContext = true;

                    AddCompulsoryParameter("characterInMarketID");
                    AddCompulsoryParameter("verificationCode");
                    AddParameter("addToDeck");
                }
                public PurchaseOffer(int characterInMarketID, string verificationCode)
                    : this()
                {
                    SetParamenterValue("characterInMarketID", characterInMarketID);
                    SetParamenterValue("verificationCode", verificationCode);
                }
                public int characterInMarketID
                {
                    get { return (int)GetParameterValue("characterInMarketID"); }
                    set { SetParamenterValue("characterInMarketID", value); }
                }
                public string verificationCode
                {
                    get { return (string)GetParameterValue("verificationCode"); }
                    set { SetParamenterValue("verificationCode", value); }
                }
                public bool addToDeck
                {
                    get { return (bool)GetParameterValue("addToDeck"); }
                    set { SetParamenterValue("addToDeck", value); }
                }
            }
            public class SellCharacter : ApiCall
            {
                private SellCharacter()
                    : base("market.sellCharacter")
                {
                    RequiresActionAccess = true;
                    ReturnsContext = true;

                    AddCompulsoryParameter("characterInCollectionID");
                    AddParameter("priceInClintz");
                    AddParameter("destNickname");
                }
                public SellCharacter(int characterInCollectionID)
                    : this()
                {
                    SetParamenterValue("characterInCollectionID", characterInCollectionID);
                }
                public int characterInCollectionID
                {
                    get { return (int)GetParameterValue("characterInCollectionID"); }
                    set { SetParamenterValue("characterInCollectionID", value); }
                }
                public int priceInClintz
                {
                    get { return (int)GetParameterValue("priceInClintz"); }
                    set { SetParamenterValue("priceInClintz", value); }
                }
                public string destNickname
                {
                    get { return (string)GetParameterValue("destNickname"); }
                    set { SetParamenterValue("destNickname", value); }
                }
            }
            public class SellCharacterToBank : ApiCall
            {
                private SellCharacterToBank()
                    : base("market.sellCharacterToBank")
                {
                    RequiresActionAccess = true;
                    ReturnsContext = true;

                    AddCompulsoryParameter("characterInCollectionID");
                }
                public SellCharacterToBank(int characterInCollectionID)
                    : this()
                {
                    SetParamenterValue("characterInCollectionID", characterInCollectionID);
                }
                public int characterInCollectionID
                {
                    get { return (int)GetParameterValue("characterInCollectionID"); }
                    set { SetParamenterValue("characterInCollectionID", value); }
                }
            }
        }
        public class Missions
        {
            public class GetCategories : ApiCall
            {
                public GetCategories()
                    : base("missions.getCategories")
                {
                    RequiresUserAccess = true;
                    ReturnsContext = true;
                    ReturnsItems = true;
                }
            }
            public class GetMissions : ApiCall
            {
                public GetMissions()
                    : base("missions.getMissions")
                {
                    RequiresUserAccess = true;
                    ReturnsContext = true;
                    ReturnsItems = true;

                    AddParameter("group");
                }
                public string group
                {
                    get { return (string)GetParameterValue("group"); }
                    set { SetParamenterValue("group", value); }
                }
            }
            public class GetMissionsInCategory : ApiCall
            {
                private GetMissionsInCategory()
                    : base("missions.getMissionsInCategory")
                {
                    RequiresUserAccess = true;
                    ReturnsContext = true;
                    ReturnsItems = true;

                    AddCompulsoryParameter("categoryID");
                }
                public GetMissionsInCategory(int categoryID)
                    : this()
                {
                    SetParamenterValue("categoryID", categoryID);
                }
                public int categoryID
                {
                    get { return (int)GetParameterValue("categoryID"); }
                    set { SetParamenterValue("categoryID", value); }
                }
            }
            public class getMissionsRanking : ApiCall
            {
                public getMissionsRanking()
                    : base("missions.getMissionsRanking")
                {
                    RequiresUserAccess = true;
                    ReturnsContext = true;
                    ReturnsItems = true;

                    AddParameter("imageSize");
                    AddParameter("imageFormat");
                }
                public string imageSize
                {
                    get { return (string)GetParameterValue("imageSize"); }
                    set { SetParamenterValue("imageSize", value); }
                }
                public string imageFormat
                {
                    get { return (string)GetParameterValue("imageFormat"); }
                    set { SetParamenterValue("imageFormat", value); }
                }
            }
            public class GetMissionsSummary : ApiCall
            {
                public GetMissionsSummary()
                    : base("missions.getMissionsSummary")
                {
                    RequiresUserAccess = true;
                    ReturnsContext = true;
                }
            }
        }
        public class Players
        {
            public class AddFriend : ApiCall
            {
                private AddFriend()
                    : base("players.addFriend")
                {
                    RequiresUserAccess = true;
                    ReturnsContext = true;

                    AddCompulsoryParameter("playerID");
                }
                public AddFriend(int playerID)
                    : this()
                {
                    SetParamenterValue("playerID", playerID);
                }
                public int playerID
                {
                    get { return (int)GetParameterValue("playerID"); }
                    set { SetParamenterValue("playerID", value); }
                }
            }
            public class AddPlayerToPersonnalBlacklist : ApiCall
            {
                private AddPlayerToPersonnalBlacklist()
                    : base("players.addPlayerToPersonnalBlacklist")
                {
                    RequiresUserAccess = true;
                    ReturnsContext = true;

                    AddCompulsoryParameter("playerID");
                }
                public AddPlayerToPersonnalBlacklist(int playerID)
                    : this()
                {
                    SetParamenterValue("playerID", playerID);
                }
                public int playerID
                {
                    get { return (int)GetParameterValue("playerID"); }
                    set { SetParamenterValue("playerID", value); }
                }
            }
            public class DeletePrivatesMessages : ApiCall
            {
                private DeletePrivatesMessages()
                    : base("players.deletePrivatesMessages")
                {
                    RequiresActionAccess = true;
                    ReturnsContext = true;

                    AddCompulsoryParameter("privateMessageIDs");
                    AddParameter("msgBox");
                    AddParameter("emptyMsgBox");
                }
                public DeletePrivatesMessages(List<int> privateMessageIDs)
                    : this()
                {
                    SetParamenterValue("privateMessageIDs", privateMessageIDs);
                }
                public List<int> privateMessageIDs
                {
                    get { return (List<int>)GetParameterValue("privateMessageIDs"); }
                    set { SetParamenterValue("privateMessageIDs", value); }
                }
                public string msgBox
                {
                    get { return (string)GetParameterValue("msgBox"); }
                    set { SetParamenterValue("msgBox", value); }
                }
                public bool emptyMsgBox
                {
                    get { return (bool)GetParameterValue("emptyMsgBox"); }
                    set { SetParamenterValue("emptyMsgBox", value); }
                }
            }
            public class GetAppNotificationsContent : ApiCall
            {
                private GetAppNotificationsContent()
                    : base("players.getAppNotificationsContent")
                {
                    RequiresUserAccess = true;
                    ReturnsContext = true;

                    AddCompulsoryParameter("appNotifIDs");
                }
                public GetAppNotificationsContent(int appNotifIDs)
                    : this()
                {
                    SetParamenterValue("appNotifIDs", appNotifIDs);
                }
                public int appNotifIDs
                {
                    get { return (int)GetParameterValue("appNotifIDs"); }
                    set { SetParamenterValue("appNotifIDs", value); }
                }
            }
            public class GetBattleHistoryPage : ApiCall
            {
                public GetBattleHistoryPage()
                    : base("players.getBattleHistoryPage")
                {
                    RequiresUserAccess = true;
                    ReturnsContext = true;
                    ReturnsItems = true;

                    AddParameter("page");
                    AddParameter("nbPerPage");
                    AddParameter("battleRuleID");
                }
                public int page
                {
                    get { return (int)GetParameterValue("page"); }
                    set { SetParamenterValue("page", value); }
                }
                public int nbPerPage
                {
                    get { return (int)GetParameterValue("nbPerPage"); }
                    set { SetParamenterValue("nbPerPage", value); }
                }
                public int battleRuleID
                {
                    get { return (int)GetParameterValue("battleRuleID"); }
                    set { SetParamenterValue("battleRuleID", value); }
                }
            }
            public class GetBattleRuleInfos : ApiCall
            {
                private GetBattleRuleInfos()
                    : base("players.getBattleRuleInfos")
                {
                    RequiresUserAccess = true;
                    ReturnsContext = true;

                    AddCompulsoryParameter("battleRuleID");
                }
                public GetBattleRuleInfos(int battleRuleID)
                    : this()
                {
                    SetParamenterValue("battleRuleID", battleRuleID);
                }
                public int battleRuleID
                {
                    get { return (int)GetParameterValue("battleRuleID"); }
                    set { SetParamenterValue("battleRuleID", value); }
                }
            }
            public class GetFeed : ApiCall
            {
                public GetFeed()
                    : base("players.getFeed")
                {
                    RequiresUserAccess = true;
                    ReturnsContext = true;
                    ReturnsItems = true;

                    AddParameter("nbStoriesMax");
                    AddParameter("highestID");
                }
                public int nbStoriesMax
                {
                    get { return (int)GetParameterValue("nbStoriesMax"); }
                    set { SetParamenterValue("nbStoriesMax", value); }
                }
                public int highestID
                {
                    get { return (int)GetParameterValue("highestID"); }
                    set { SetParamenterValue("highestID", value); }
                }
            }
            public class GetFeedFilters : ApiCall
            {
                public GetFeedFilters()
                    : base("players.getFeedFilters")
                {
                    RequiresUserAccess = true;
                    ReturnsContext = true;
                    ReturnsItems = true;
                }
            }
            public class GetFriends : ApiCall
            {
                public GetFriends()
                    : base("players.getFriends")
                {
                    RequiresUserAccess = true;
                    ReturnsContext = true;
                    ReturnsItems = true;

                    AddParameter("imageSize");
                    AddParameter("imageFormat");
                }
                public string imageSize
                {
                    get { return (string)GetParameterValue("imageSize"); }
                    set { SetParamenterValue("imageSize", value); }
                }
                public string imageFormat
                {
                    get { return (string)GetParameterValue("imageFormat"); }
                    set { SetParamenterValue("imageFormat", value); }
                }
            }
            public class GetFriendsLight : ApiCall
            {
                public GetFriendsLight()
                    : base("players.getFriends")
                {
                    RequiresUserAccess = true;
                    ReturnsContext = true;
                    ReturnsItems = true;
                }
            }
            public class GetNextDuelObjective : ApiCall
            {
                public GetNextDuelObjective()
                    : base("players.getNextDuelObjective")
                {
                    RequiresUserAccess = true;
                    ReturnsContext = true;

                    AddParameter("imageSize");
                    AddParameter("imageFormat");
                }
                public string imageSize
                {
                    get { return (string)GetParameterValue("imageSize"); }
                    set { SetParamenterValue("imageSize", value); }
                }
                public string imageFormat
                {
                    get { return (string)GetParameterValue("imageFormat"); }
                    set { SetParamenterValue("imageFormat", value); }
                }
            }
            public class GetPersonnalBlacklist : ApiCall
            {
                public GetPersonnalBlacklist()
                    : base("players.getPersonnalBlacklist")
                {
                    RequiresUserAccess = true;
                    ReturnsContext = true;
                    ReturnsItems = true;
                }
            }
            public class GetPlayerProfile : ApiCall
            {
                private GetPlayerProfile()
                    : base("players.getPlayerProfile")
                {
                    RequiresUserAccess = true;
                    ReturnsContext = true;

                    AddCompulsoryParameter("playerID");
                    AddParameter("imageSize");
                    AddParameter("imageFormat");
                }
                public GetPlayerProfile(int playerID)
                    : this()
                {
                    SetParamenterValue("playerID", playerID);
                }
                public int playerID
                {
                    get { return (int)GetParameterValue("playerID"); }
                    set { SetParamenterValue("playerID", value); }
                }
                public string imageSize
                {
                    get { return (string)GetParameterValue("imageSize"); }
                    set { SetParamenterValue("imageSize", value); }
                }
                public string imageFormat
                {
                    get { return (string)GetParameterValue("imageFormat"); }
                    set { SetParamenterValue("imageFormat", value); }
                }
            }
            public class GetPrivatesMessages : ApiCall
            {
                public GetPrivatesMessages()
                    : base("players.getPrivatesMessages")
                {
                    RequiresActionAccess = true;
                    ReturnsContext = true;
                    ReturnsItems = true;

                    AddParameter("page");
                    AddParameter("nbPerPage");
                    AddParameter("msgBox");
                    AddParameter("unreadOnly");
                }
                public int page
                {
                    get { return (int)GetParameterValue("page"); }
                    set { SetParamenterValue("page", value); }
                }
                public int nbPerPage
                {
                    get { return (int)GetParameterValue("nbPerPage"); }
                    set { SetParamenterValue("nbPerPage", value); }
                }
                public string msgBox
                {
                    get { return (string)GetParameterValue("msgBox"); }
                    set { SetParamenterValue("msgBox", value); }
                }
                public bool unreadOnly
                {
                    get { return (bool)GetParameterValue("unreadOnly"); }
                    set { SetParamenterValue("unreadOnly", value); }
                }
            }
            public class GetTotalOnlineFriends : ApiCall
            {
                public GetTotalOnlineFriends()
                    : base("players.getTotalOnlineFriends")
                {
                    RequiresUserAccess = true;
                    ReturnsContext = true;
                }
            }
            public class GetTotalUnreadPrivatesMessages : ApiCall
            {
                public GetTotalUnreadPrivatesMessages()
                    : base("players.getTotalUnreadPrivatesMessages")
                {
                    RequiresUserAccess = true;
                    ReturnsContext = true;
                }
            }
            public class GetTutorialProgress : ApiCall
            {
                public GetTutorialProgress()
                    : base("players.getTutorialProgress")
                {
                    RequiresUserAccess = true;
                    ReturnsContext = true;
                }
            }
            public class GetTutorialTips : ApiCall
            {
                public GetTutorialTips()
                    : base("players.getTutorialTips")
                {
                    RequiresUserAccess = true;
                    ReturnsContext = true;
                    ReturnsItems = true;
                }
            }
            public class RemoveFriend : ApiCall
            {
                private RemoveFriend()
                    : base("players.removeFriend")
                {
                    RequiresUserAccess = true;
                    ReturnsContext = true;

                    AddCompulsoryParameter("playerID");
                }
                public RemoveFriend(int playerID)
                    : this()
                {
                    SetParamenterValue("playerID", playerID);
                }
                public int playerID
                {
                    get { return (int)GetParameterValue("playerID"); }
                    set { SetParamenterValue("playerID", value); }
                }
            }
            public class RemovePlayerFromPersonnalBlacklist : ApiCall
            {
                private RemovePlayerFromPersonnalBlacklist()
                    : base("players.removePlayerFromPersonnalBlacklist")
                {
                    RequiresUserAccess = true;
                    ReturnsContext = true;

                    AddCompulsoryParameter("playerID");
                }
                public RemovePlayerFromPersonnalBlacklist(int playerID)
                    : this()
                {
                    SetParamenterValue("playerID", playerID);
                }
                public int playerID
                {
                    get { return (int)GetParameterValue("playerID"); }
                    set { SetParamenterValue("playerID", value); }
                }
            }
            public class SendPlayerLostPasswordMail : ApiCall
            {
                private SendPlayerLostPasswordMail()
                    : base("players.sendPlayerLostPasswordMail")
                {
                    RequiresUserAccess = true;
                    ReturnsContext = true;

                    AddCompulsoryParameter("pseudo");
                }
                public SendPlayerLostPasswordMail(string pseudo)
                    : this()
                {
                    SetParamenterValue("pseudo", pseudo);
                }
                public string pseudo
                {
                    get { return (string)GetParameterValue("pseudo"); }
                    set { SetParamenterValue("pseudo", value); }
                }
            }
            public class SendPrivateMessage : ApiCall
            {
                private SendPrivateMessage()
                    : base("players.sendPrivateMessage")
                {
                    RequiresActionAccess = true;
                    ReturnsContext = true;

                    AddCompulsoryParameter("destPlayerIDorPlayerNickname");
                    AddCompulsoryParameter("msgBody");
                    AddParameter("msgSubject");
                }
                public SendPrivateMessage(string destPlayerIDorPlayerNickname, string msgBody)
                    : this()
                {
                    SetParamenterValue("destPlayerIDorPlayerNickname", destPlayerIDorPlayerNickname);
                    SetParamenterValue("msgBody", msgBody);
                }
                public string destPlayerIDorPlayerNickname
                {
                    get { return (string)GetParameterValue("destPlayerIDorPlayerNickname"); }
                    set { SetParamenterValue("destPlayerIDorPlayerNickname", value); }
                }
                public string msgBody
                {
                    get { return (string)GetParameterValue("msgBody"); }
                    set { SetParamenterValue("msgBody", value); }
                }
                public string msgSubject
                {
                    get { return (string)GetParameterValue("msgSubject"); }
                    set { SetParamenterValue("msgSubject", value); }
                }
                
            }
            public class SetFeedFilter : ApiCall
            {
                private SetFeedFilter()
                    : base("players.setFeedFilter")
                {
                    RequiresUserAccess = true;
                    ReturnsContext = true;

                    AddCompulsoryParameter("activityTypeID");
                    AddCompulsoryParameter("filterState");
                    AddParameter("filterKind");
                }
                public SetFeedFilter(int activityTypeID, bool filterState)
                    : this()
	            {
                    SetParamenterValue("activityTypeID", activityTypeID);
                    SetParamenterValue("filterState", filterState);
	            }
                public int activityTypeID
                {
                    get { return (int)GetParameterValue("activityTypeID"); }
                    set { SetParamenterValue("activityTypeID", value); }
                }
                public bool filterState
                {
                    get { return (bool)GetParameterValue("filterState"); }
                    set { SetParamenterValue("filterState", value); }
                }
                public int filterKind
                {
                    get { return (int)GetParameterValue("filterKind"); }
                    set { SetParamenterValue("filterKind", value); }
                }
            }
            public class SetLanguages : ApiCall
            {
                private SetLanguages()
                    : base("players.setLanguage")
	            {
                    RequiresUserAccess = true;
                    ReturnsContext = true;

                    AddCompulsoryParameter("languages");
	            }
                public SetLanguages(List<string> languages)
                    : this()
                {
                    SetParamenterValue("languages", languages);
                }
                public List<string> languages
                {
                    get { return (List<string>)GetParameterValue("languages"); }
                    set { SetParamenterValue("languages", value); }
                }
            }
            public class SetPicture : ApiCall
            {
                private SetPicture()
                    : base("players.setPicture")
                {
                    RequiresUserAccess = true;
                    ReturnsContext = true;

                    AddCompulsoryParameter("fileInBase64");
                }
                /// <summary>
                /// Use <see cref="UrSimulator.ApiManagement.Utils.Base64Converter.ConvertImageFileToBase64(string)"/> to convert an image to a valid Base64 string before calling the constructor.
                /// </summary>
                /// <param name="fileInBase64"></param>
                public SetPicture(string fileInBase64)
                    : this()
                {
                    SetParamenterValue("fileInBase64", fileInBase64);
                }
                /// <summary>
                /// Use <see cref="UrSimulator.ApiManagement.Utils.Base64Converter.ConvertImageFileToBase64(string)"/> to convert an image to a valid Base64 string before calling the constructor.
                /// </summary>
                public string fileInBase64
                {
                    get { return (string)GetParameterValue("fileInBase64"); }
                    set { SetParamenterValue("fileInBase64", value); }
                }
            }
            public class SetPrivatesMessagesReadStatus : ApiCall
            {
                private SetPrivatesMessagesReadStatus()
                    : base("players.setPrivatesMessagesReadStatus")
	            {
                    RequiresActionAccess = true;
                    ReturnsContext = true;

                    AddCompulsoryParameter("privateMessageIDs");
                    AddParameter("msgBox");
                    AddParameter("readStatus");
	            }
                public SetPrivatesMessagesReadStatus(List<int> privateMessageIDs)
                    : this()
                {
                    SetParamenterValue("privateMessageIDs", privateMessageIDs);
                }
                public List<int> privateMessageIDs
                {
                    get { return (List<int>)GetParameterValue("privateMessageIDs"); }
                    set { SetParamenterValue("privateMessageIDs", value); }
                }
                public string msgBox
                {
                    get { return (string)GetParameterValue("msgBox"); }
                    set { SetParamenterValue("msgBox", value); }
                }
                public bool readStatus
                {
                    get { return (bool)GetParameterValue("readStatus"); }
                    set { SetParamenterValue("readStatus", value); }
                }
            }
            public class SetSpecialOfferSeen : ApiCall
            {
                private SetSpecialOfferSeen()
                    : base("players.setSpecialOfferSeen")
                {
                    RequiresUserAccess = true;
                    ReturnsContext = true;
                    ReturnsItems = true;

                    AddCompulsoryParameter("id_offer");
                    AddCompulsoryParameter("is_accepted");
                }
                public SetSpecialOfferSeen(int id_offer, bool is_accepted)
                    : this()
                {
                    SetParamenterValue("id_offer", id_offer);
                    SetParamenterValue("is_accepted", is_accepted);
                }
                public int id_offer
                {
                    get { return (int)GetParameterValue("id_offer"); }
                    set { SetParamenterValue("id_offer", value); }
                }
                public bool is_accepted
                {
                    get { return (bool)GetParameterValue("is_accepted"); }
                    set { SetParamenterValue("is_accepted", value); }
                }
            }
            public class SetTutorialTipSeen : ApiCall
            {
                public SetTutorialTipSeen()
                    : base("players.setTutorialTipSeen")
                {
                    RequiresUserAccess = true;
                    ReturnsContext = true;
                    ReturnsItems = true;

                    AddParameter("codename");
                }
                public string codename
                {
                    get { return (string)GetParameterValue("codename"); }
                    set { SetParamenterValue("codename", codename); }
                }
            }
            public class WriteFeedMsg : ApiCall
            {
                public WriteFeedMsg()
                    : base("players.writeFeedMsg")
	            {
                    RequiresUserAccess = true;
                    ReturnsContext = true;

                    AddParameter("msg");
	            }
                public string msg
                {
                    get { return (string)GetParameterValue("msg"); }
                    set { SetParamenterValue("msg", value); }
                }
                
            }
        }
        public class Urc
        {
            public class GetCharacters : ApiCall
            {
                public GetCharacters()
                    : base("urc.getCharacters")
                {
                    RequiresPublicAccess = true;
                    ReturnsContext = true;
                    ReturnsItems = true;
                }
            }
            public class GetClans : ApiCall
            {
                public GetClans()
                    : base("urc.getClans")
                {
                    RequiresPublicAccess = true;
                    ReturnsContext = true;
                    ReturnsItems = true;
                }
            }
        }
    }
}

