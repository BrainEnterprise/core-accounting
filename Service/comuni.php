<?php

/*
    DICHIARAZIONE VARIABILI
*/
$arrayComuni = array();
$filterCodiceCatastale = null;
$filterRegione = null;
$filterArea = null;
$filterSiglaAutomobilistica = null;
$errorMessage = "";

header('Content-type: application/json');

//Make sure that it is a POST request.
if(strcasecmp($_SERVER['REQUEST_METHOD'], 'POST') != 0){
    throw new Exception('Request method must be POST!');
}

$contentType = isset($_SERVER["CONTENT_TYPE"]) ? trim($_SERVER["CONTENT_TYPE"]) : '';
if(strcasecmp($contentType, 'application/json') == 0){
    //Receive the RAW post data.
    $content = trim(file_get_contents('php://input'));

    //Attempt to decode the incoming RAW post data from JSON.
    $decoded = json_decode($content);

    
    if(isset($decoded->{'codiceArea'})) {
        $filterArea = $decoded->{'codiceArea'};
    }

    if(isset($decoded->{'codiceRegione'})) {
        $filterRegione = $decoded->{'codiceRegione'};
        $filterArea = null;
    }

    if(isset($decoded->{'siglaAutomobilistica'})) {
        $filterSiglaAutomobilistica = $decoded->{'siglaAutomobilistica'};
        $filterArea = null;
        $filterRegione = null;
    }
}

if(isset($_REQUEST['codiceArea']) && ($_REQUEST['codiceArea'] != null || $_REQUEST['codiceArea'] != "")) {
    $filterArea = $_REQUEST['codiceArea'];
}

if(isset($_REQUEST['codiceRegione']) && ($_REQUEST['codiceRegione'] != null || $_REQUEST['codiceRegione'] != "")) {
    $filterRegione = $_REQUEST['codiceRegione'];
    $filterArea = null;
}

if(isset($_REQUEST['siglaAutomobilistica']) && ($_REQUEST['siglaAutomobilistica'] != null || $_REQUEST['siglaAutomobilistica'] != "")) {
    $filterSiglaAutomobilistica = $_REQUEST['siglaAutomobilistica'];
    $filterArea = null;
    $filterRegione = null;
}

if(isset($_REQUEST['codiceCatastale']) && ($_REQUEST['codiceCatastale'] != null || $_REQUEST['codiceCatastale'] != "")) {
    $filterCodiceCatastale = $_REQUEST['codiceCatastale'];
    $filterArea = null;
    $filterRegione = null;
    $filterSiglaAutomobilistica = null;
}

try {
    if (($handle = fopen("Elenco-comuni-italiani.csv", "r")) !== FALSE) {
        //Consumo la riga dell'header
        fgetcsv($handle, 10000, ";");

        if($filterCodiceCatastale != null) {
			$errorMessage = "CODICE CATASTALE NON VALIDO/NON TROVATO!";
            while (($data = fgetcsv($handle, 1000, ";")) !== FALSE) {
                if(trim($data[19]) == $filterCodiceCatastale) {
                    $temp = array('codiceRegione' => $data[0],
                                //'codiceUnitàTerritoriale' => $data[1],
                                //'codiceProvincia' => $data[2],
                                //'progressivoComune' => $data[3],
                                //'codiceIstatAlfanumerico' => $data[4],
                                //'descrizioneMulti' => utf8_encode($data[5]),
                                'descrizioneItaliana' => utf8_encode($data[6]),
                                //'descrizioneAltraLingua' => utf8_encode($data[7]),
                                'codiceArea' => $data[8],
                                'descrizioneArea' => $data[9],
                                'descrizioneRegione' => utf8_encode($data[10]),
                                'descrizioneUnitaTerritoriale' => utf8_encode($data[11]),
                                //'tipologiaUnitaTerritoriale' => $data[12],
                                //'flagComuneCapoluogo' => $data[13],
                                'siglaAutomobilistica' => $data[14],
                                //'codiceComuneAlfanumerico' => $data[15],
                                // 'codiceComune2016' => $data[16],
                                // 'codiceComune2009' => $data[17],
                                // 'codiceComune2005' => $data[18],
                                'codiceCatastale' => $data[19],
                                // 'codiceNUTS1_2010' => $data[20],
                                // 'codiceNUTS2_2010' => $data[21],
                                // 'codiceNUTS3_2010' => $data[22],
                                // 'codiceNUTS1_2021' => $data[23],
                                // 'codiceNUTS2_2021' => $data[24],
                                // 'codiceNUTS3_2021' => $data[25],
                                );
                        array_push($arrayComuni, $temp);
						$errorMessage = "";
                        break;
                }
            }            
        }
       
        else {
            if($filterSiglaAutomobilistica != null) {
                while (($data = fgetcsv($handle, 1000, ";")) !== FALSE) {
                    if(trim($data[14]) == $filterSiglaAutomobilistica) {
                        $temp = array('codiceRegione' => $data[0],
                                //'codiceUnitàTerritoriale' => $data[1],
                                //'codiceProvincia' => $data[2],
                                //'progressivoComune' => $data[3],
                                //'codiceIstatAlfanumerico' => $data[4],
                                //'descrizioneMulti' => utf8_encode($data[5]),
                                'descrizioneItaliana' => utf8_encode($data[6]),
                                //'descrizioneAltraLingua' => utf8_encode($data[7]),
                                'codiceArea' => $data[8],
                                'descrizioneArea' => $data[9],
                                'descrizioneRegione' => utf8_encode($data[10]),
                                'descrizioneUnitaTerritoriale' => utf8_encode($data[11]),
                                //'tipologiaUnitaTerritoriale' => $data[12],
                                //'flagComuneCapoluogo' => $data[13],
                                'siglaAutomobilistica' => $data[14],
                                //'codiceComuneAlfanumerico' => $data[15],
                                // 'codiceComune2016' => $data[16],
                                // 'codiceComune2009' => $data[17],
                                // 'codiceComune2005' => $data[18],
                                'codiceCatastale' => $data[19],
                                // 'codiceNUTS1_2010' => $data[20],
                                // 'codiceNUTS2_2010' => $data[21],
                                // 'codiceNUTS3_2010' => $data[22],
                                // 'codiceNUTS1_2021' => $data[23],
                                // 'codiceNUTS2_2021' => $data[24],
                                // 'codiceNUTS3_2021' => $data[25],
                            );
                    array_push($arrayComuni, $temp);
                    }
                }    
            }   
            else if($filterRegione != null) {
                while (($data = fgetcsv($handle, 1000, ";")) !== FALSE) {
                    if(trim($data[0]) == $filterRegione) {
                        $temp = array('codiceRegione' => $data[0],
                        //'codiceUnitàTerritoriale' => $data[1],
                        //'codiceProvincia' => $data[2],
                        //'progressivoComune' => $data[3],
                        //'codiceIstatAlfanumerico' => $data[4],
                        //'descrizioneMulti' => utf8_encode($data[5]),
                        'descrizioneItaliana' => utf8_encode($data[6]),
                        //'descrizioneAltraLingua' => utf8_encode($data[7]),
                        'codiceArea' => $data[8],
                        'descrizioneArea' => $data[9],
                        'descrizioneRegione' => utf8_encode($data[10]),
                        'descrizioneUnitaTerritoriale' => utf8_encode($data[11]),
                        //'tipologiaUnitaTerritoriale' => $data[12],
                        //'flagComuneCapoluogo' => $data[13],
                        'siglaAutomobilistica' => $data[14],
                        //'codiceComuneAlfanumerico' => $data[15],
                        // 'codiceComune2016' => $data[16],
                        // 'codiceComune2009' => $data[17],
                        // 'codiceComune2005' => $data[18],
                        'codiceCatastale' => $data[19],
                        // 'codiceNUTS1_2010' => $data[20],
                        // 'codiceNUTS2_2010' => $data[21],
                        // 'codiceNUTS3_2010' => $data[22],
                        // 'codiceNUTS1_2021' => $data[23],
                        // 'codiceNUTS2_2021' => $data[24],
                        // 'codiceNUTS3_2021' => $data[25],
                        );
                        array_push($arrayComuni, $temp);
                    }
                }
            }   
            else if($filterArea != null) {
                while (($data = fgetcsv($handle, 1000, ";")) !== FALSE) {
                    if(trim($data[8]) == $filterArea) {
                        $temp = array('codiceRegione' => $data[0],
                        //'codiceUnitàTerritoriale' => $data[1],
                        //'codiceProvincia' => $data[2],
                        //'progressivoComune' => $data[3],
                        //'codiceIstatAlfanumerico' => $data[4],
                        //'descrizioneMulti' => utf8_encode($data[5]),
                        'descrizioneItaliana' => utf8_encode($data[6]),
                        //'descrizioneAltraLingua' => utf8_encode($data[7]),
                        'codiceArea' => $data[8],
                        'descrizioneArea' => $data[9],
                        'descrizioneRegione' => utf8_encode($data[10]),
                        'descrizioneUnitaTerritoriale' => utf8_encode($data[11]),
                        //'tipologiaUnitaTerritoriale' => $data[12],
                        //'flagComuneCapoluogo' => $data[13],
                        'siglaAutomobilistica' => $data[14],
                        //'codiceComuneAlfanumerico' => $data[15],
                        // 'codiceComune2016' => $data[16],
                        // 'codiceComune2009' => $data[17],
                        // 'codiceComune2005' => $data[18],
                        'codiceCatastale' => $data[19],
                        // 'codiceNUTS1_2010' => $data[20],
                        // 'codiceNUTS2_2010' => $data[21],
                        // 'codiceNUTS3_2010' => $data[22],
                        // 'codiceNUTS1_2021' => $data[23],
                        // 'codiceNUTS2_2021' => $data[24],
                        // 'codiceNUTS3_2021' => $data[25],
                        );
                        array_push($arrayComuni, $temp);
                    }
                }
            }  
            else {
                while (($data = fgetcsv($handle, 1000, ";")) !== FALSE) {
                    $temp = array('codiceRegione' => $data[0],
                    //'codiceUnitàTerritoriale' => $data[1],
                    //'codiceProvincia' => $data[2],
                    //'progressivoComune' => $data[3],
                    //'codiceIstatAlfanumerico' => $data[4],
                    //'descrizioneMulti' => utf8_encode($data[5]),
                    'descrizioneItaliana' => utf8_encode($data[6]),
                    //'descrizioneAltraLingua' => utf8_encode($data[7]),
                    'codiceArea' => $data[8],
                    'descrizioneArea' => $data[9],
                    'descrizioneRegione' => utf8_encode($data[10]),
                    'descrizioneUnitaTerritoriale' => utf8_encode($data[11]),
                    //'tipologiaUnitaTerritoriale' => $data[12],
                    //'flagComuneCapoluogo' => $data[13],
                    'siglaAutomobilistica' => $data[14],
                    //'codiceComuneAlfanumerico' => $data[15],
                    // 'codiceComune2016' => $data[16],
                    // 'codiceComune2009' => $data[17],
                    // 'codiceComune2005' => $data[18],
                    'codiceCatastale' => $data[19],
                    // 'codiceNUTS1_2010' => $data[20],
                    // 'codiceNUTS2_2010' => $data[21],
                    // 'codiceNUTS3_2010' => $data[22],
                    // 'codiceNUTS1_2021' => $data[23],
                    // 'codiceNUTS2_2021' => $data[24],
                    // 'codiceNUTS3_2021' => $data[25],
                    );
                    array_push($arrayComuni, $temp);
                }
            }
        }   
        fclose($handle); 
    }
}
catch(Exception $ex) {
    $errorMessage = $ex->getMessagge();
}

if(empty($arrayComuni) && $errorMessage == "") {
    $errorMessage = "NESSUN COMUNE TROVATO";
}

$toReturn = array('Result' => $arrayComuni, 'Status' => http_response_code(), 'ErrorMessagge' => $errorMessage);
$jsonToReturn = json_encode($toReturn);
echo $jsonToReturn;
?>
