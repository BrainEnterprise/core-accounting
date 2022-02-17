<?php
/*
    VARIABLE DECLARATION
*/
$filter = null;
$arrayRegioni = array();
$errorMessagge = "";

//Make sure that it is a POST request.
if(strcasecmp($_SERVER['REQUEST_METHOD'], 'POST') != 0){
    throw new Exception('Request method must be POST!');
}

try {
    //Receive the RAW post data.
    $content = trim(file_get_contents('php://input'));

    $contentType = isset($_SERVER["CONTENT_TYPE"]) ? trim($_SERVER["CONTENT_TYPE"]) : '';
    if(strcasecmp($contentType, 'application/json') == 0){
        $decoded = json_decode($content);
        
        if(isset($decoded->{'codiceArea'})) {
            $filter = $decoded->{'codiceArea'};
        }
    }

    if(isset($_REQUEST['codiceArea']) && ($_REQUEST['codiceArea'] != null || $_REQUEST['codiceArea'] != "")) {
        $filter = $_REQUEST['codiceArea'];
    }



    if (($handle = fopen("Elenco-regioni.csv", "r")) !== FALSE) {
        //Consumo la riga dell'header
        fgetcsv($handle, 10000, ",");

        while (($data = fgetcsv($handle, 1000, ";")) !== FALSE) {
            if($filter != null && $filter == $data[1]) {
                $temp = array ('codiceArea' => $data[1], 
                        'descrizioneArea' => $data[2],
                        'codiceRegione' => $data[0],
                        'descrizioneRegione' => utf8_encode($data[3])
                    );
                    array_push($arrayRegioni, $temp);
            }
            else if($filter == null){
                $temp = array ('codiceArea' => $data[1], 
                        'descrizioneArea' => $data[2],
                        'codiceRegione' => $data[0],
                        'descrizioneRegione' => utf8_encode($data[3])
                    );
                    array_push($arrayRegioni, $temp);
            }

        }
        fclose($handle);
    }
}
catch(Exception $ex) {
    $errorMessagge = $ex->getMessagge();
}

if(empty($arrayRegioni) && $errorMessagge == "") {
    $errorMessagge = "NESSUNA REGIONE TROVATA!";
}

$toReturn = array('Result' => $arrayRegioni, 'Status' => http_response_code(), 'ErrorMessagge' => $errorMessagge);
$jsonToReturn = json_encode($toReturn);
echo $jsonToReturn;

?>