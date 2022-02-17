<?php
/*
    DICHIARAZIONE VARIABILI
    filterArea -> codice area 
    filterRegione -> codice regione
    
    In caso vengano passati entrambi, il filtro dell'area verrà ignorato 
    in quanto inutile nel filtraggio
    tramite regione!
*/
$filterArea = null;
$filterRegione = null;
$arrayProvince = array();
$errorMessagge = "";

//Make sure that it is a POST request.
if(strcasecmp($_SERVER['REQUEST_METHOD'], 'POST') != 0){
    throw new Exception('Request method must be POST!');
}

//Receive the RAW post data.
$content = trim(file_get_contents('php://input'));

$contentType = isset($_SERVER["CONTENT_TYPE"]) ? trim($_SERVER["CONTENT_TYPE"]) : '';
if(strcasecmp($contentType, 'application/json') == 0){
    $decoded = json_decode($content);
    
    if(isset($decoded->{'codiceArea'})) {
        $filter = $decoded->{'codiceArea'};
    }
    if(isset($decoded->{'codiceRegione'})) {
        $filterRegione = $decoded->{'codiceRegione'};
        $filterArea = null;
    }  
}
if(isset($_REQUEST['codiceArea']) && ($_REQUEST['codiceArea'] != null || $_REQUEST['codiceArea'] != "")) {
    $filterArea = $_REQUEST['codiceArea'];
}

if(isset($_REQUEST['codiceRegione']) && ($_REQUEST['codiceRegione'] != null || $_REQUEST['codiceRegione'] != "")) {
    $filterRegione = $_REQUEST['codiceRegione'];
    $filterArea = null;
}

try {
    if (($handle = fopen("Elenco-province.csv", "r")) !== FALSE) {
        //Consumo la riga dell'header
        fgetcsv($handle, 10000, ",");

        while (($data = fgetcsv($handle, 1000, ";")) !== FALSE) {
            if($filterRegione != null && $filterRegione == $data[0]) {
                $temp = array ('codiceArea' => $data[2],
                            'codiceUnitàTerritoriale' => $data[1], 
                            'descrizioneArea' => $data[3],
                            'codiceRegione' => $data[0],
                            'descrizioneRegione' => utf8_encode($data[4]),
                            'descrizioneUnitaTerritoriale' => utf8_encode($data[5]),
                            'siglaAutomobilistica' => $data[6]
                            );
                array_push($arrayProvince, $temp);
            }
            if($filterArea != null && $filterArea == $data[2]) {
                $temp = array ('codiceArea' => $data[2],
                            'codiceUnitàTerritoriale' => $data[1], 
                            'descrizioneArea' => $data[3],
                            'codiceRegione' => $data[0],
                            'descrizioneRegione' => utf8_encode($data[4]),
                            'descrizioneUnitaTerritoriale' => utf8_encode($data[5]),
                            'siglaAutomobilistica' => $data[6]
                            );
                array_push($arrayProvince, $temp);
            }
            if($filterArea == null && $filterRegione == null) {
                $temp = array ('codiceArea' => $data[2],
                            'codiceUnitàTerritoriale' => $data[1], 
                            'descrizioneArea' => $data[3],
                            'codiceRegione' => $data[0],
                            'descrizioneRegione' => utf8_encode($data[4]),
                            'descrizioneUnitaTerritoriale' => utf8_encode($data[5]),
                            'siglaAutomobilistica' => $data[6]
                            );
                array_push($arrayProvince, $temp);        
            }
        }
        fclose($handle);
    }
}
catch(Exception $ex) {
    $errorMessagge = $ex->getMessagge();
}

if(empty($arrayProvince) && $errorMessagge == "") {
    $errorMessagge = "NESSUNA PROVINCIA TROVATA!";
}
$toReturn = array('Result' => $arrayProvince, 'Status' => http_response_code(), 'ErrorMessagge' => $errorMessagge);
$jsonToReturn = json_encode($toReturn);
echo $jsonToReturn;

?>