<?php 
if ($_GET["code"]) {
	echo "Token was received, it is now safe to close this page<br/>";
	echo $_GET["code"];
	file_put_contents("token.txt", $_GET["code"]);
} else if ($_GET["retrieve"]) {
	while(!file_exists("token.txt")) {
		sleep(1);
	}

	echo file_get_contents("token.txt");
	unlink("token.txt");
}
?>