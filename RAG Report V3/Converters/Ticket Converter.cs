namespace RAG_Report_V3.Converters
{
    internal class TicketConverter
    {
        // Returns the number corresponding to the RAG status.
        public int GetRAGStatusId(string value)
        {
            return value switch
            {
                "Dark Red" => 1,
                "Red" => 2,
                "Yellow" => 3,
                "Green" => 4,
                "Dark Violet" => 5,
                _ => 0
            };
        }

        // Returns the RAG value.
        public string GetRAGStatus(int value)
        {
            return value switch
            {
                1 => "Dark Red",
                2 => "Red",
                3 => "Yellow",
                4 => "Green",
                5 => "Dark Violet",
                _ => ""
            };
        }

        // Returns the integration partner.
        public string GetIntegration(string value)
        {
            return value switch
            {
                "Acturis SW4" => "acturis",
                "Acturis Custom Integration Fields: Allianz-Policy" => "acturis",
                "Acturis Custom Integration Fields: Griffiths and Armour" => "acturis",
                "Acturis Custom Integration Fields: Allianz-Prospect FULL" => "acturis",
                "Acturis Custom Integration Fields: Aston Lark" => "acturis",
                "Acturis Custom Integration Fields: Chas Insurance Prospects" => "acturis",
                "Acturis V12 Toolkit142 DataDictionary512" => "acturis",
                "Acturis Custom Integration Fields: PIB Fish" => "acturis",
                "Acturis V12 Toolkit142 DataDictionary512 OPP RO" => "acturis",
                "Acturis Custom Integration Fields: Swinton" => "acturis",
                "Acturis V13 Toolkit142 DataDictionaryv7.6.3 OPP RO" => "acturis",
                "Acturis V13 Toolkit142 Extra Fields" => "acturis",
                "Acturis V13 Toolkit142 DDv7.6.3 OPP RO PIB" => "acturis",
                "Aspasia" => "aspasia",
                "Aspasia With Viewings" => "aspasia",
                "Aspasia With Property Feed and Viewings" => "aspasia",
                "Bright Logic IM - Acquaint" => "brightlogic",
                "BrightLogic - Acquaint" => "brightlogic",
                "CFP Winman" => "cfp",
                "Estates IT" => "estates_it",
                "Expert Agent" => "expert_agent",
                "OLD_Expert_Agent" => "expert_agent",
                "LIVE_Expert_Agent_oldnew_Combined" => "expert_agent",
                "Property Schema - Property File Drop" => "file_drop",
                "Property Schema - Mapped File Drop" => "file_drop",
                "Gladstone API 2016 (2)-old" => "gladstone",
                "Gladstone API 2016" => "gladstone",
                "Gladstone API 2017" => "gladstone",
                "Gnomen" => "gnomen",
                "Jupix V2" => "jupix",
                "Jupix V3" => "jupix",
                "Property Schema - Jupix" => "jupix",
                "OpenGI InfoCentrePlus 2" => "open_gi",
                "RealCube Media - Unique Fields" => "realcube",
                "ReapIT" => "reapit",
                "ProductMatching-Reapit" => "reapit",
                "*** Reapit Outbound 2018 ***" => "reapit",
                "Reapit API" => "reapit",
                "Reapit API FullDW" => "reapit",
                "Property Schema - Reapit Foundations" => "reapit_foundations",
                "Live_RentMan" => "rent_man",
                "RentMan Trigger Integration" => "rent_man",
                "DezRezRezi" => "rezi",
                "ReziBasic" => "rezi",
                "Rezi" => "rezi",
                "Property Schema - DezRez Rezi" => "rezi",
                "SSP Electra M3 and Select" => "ssp",
                "LIVE_SSP_Pure" => "ssp",
                "SSP Electra (Version 7) For CIA" => "ssp",
                "LIVE_SSP_Pure_Endsleigh" => "ssp",
                "Vebra Alto" => "vebra_alto",
                "Vebra Alto (with RelatedObjects)" => "vebra_alto",
                "Vebra Alto (with RelatedObjects) GDPR" => "vebra_alto",
                "OLD_Vebra Live (Related Object Model)" => "vebra_live",
                "Vebra Live (Related Objects)" => "vebra_live",
                "LIVE_Veco_2015" => "veco",
                "Veco Fetherstone Leigh 2016" => "veco",
                "Veco_2016 v2" => "veco",
                "Veco_2016 v2 Century21 Only" => "veco",
                "Veco IM - Linley and Simpson Only" => "veco",
                "Veco_2020 v1 Standard" => "veco",
                _ => "default"
            };
        }
    }
}
