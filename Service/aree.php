<?php
/*
    DICHIARAZIONE VARIABILI
*/
$arrayAree = array();
$errorMessagge = "";

//Make sure that it is a POST request.
 if(strcasecmp($_SERVER['REQUEST_METHOD'], 'POST') != 0){
     throw new Exception('Request method must be POST!');
 }


//Receive the RAW post data.
$content = trim(file_get_contents('php://input'));
try {
    if (($handle = fopen("Elenco-ripartizioni.csv", "r")) !== FALSE) {
        //Consumo la riga dell'header
        fgetcsv($handle, 10000, ",");
        while (($data = fgetcsv($handle, 1000, ";")) !== FALSE) {
            $temp = array('codiceArea' => $data[0], 'descrizioneArea' => $data[1]);
            array_push($arrayAree, $temp);
        }
        fclose($handle);
    }
}
catch(Exception $ex) {
    $errorMessagge = $ex->getMessagge();
}

$toReturn = array('Result' => $arrayAree, 'Status' => http_response_code(), 'ErrorMessagge' => $errorMessagge);

header('Content-type: application/json');
$jsonToReturn = json_encode($toReturn);
echo $jsonToReturn;

?>