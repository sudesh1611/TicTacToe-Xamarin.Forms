<?php
$xmldoc = new DomDocument( '1.0' );
$xmldoc->preserveWhiteSpace = false;
$xmldoc->formatOutput = true;

$accnum = "27";
$name = "1200";
$category = "Mapsa main";

if( $xml = file_get_contents( 'account.xml') ) {
    $xmldoc->loadXML( $xml, LIBXML_NOBLANKS );

    $root = $xmldoc->getElementsByTagName('accounts')->item(0);

    $account = $xmldoc->createElement('accounts');
    $numAttribute = $xmldoc->createAttribute("account_number");
    $numAttribute->value = $accnum;
    $account->appendChild($numAttribute);

    $root->insertBefore( $account, $root->lastChild );

    // create other elements and add it to the <product> tag.
    $nameElement = $xmldoc->createElement('balance');
    $account->appendChild($nameElement);
    $nameText = $xmldoc->createTextNode($name);
    $nameElement->appendChild($nameText);
	
    $categoryElement = $xmldoc->createElement('Branch_name');
    $account->appendChild($categoryElement);
    $categoryText = $xmldoc->createTextNode($category);
    $categoryElement->appendChild($categoryText);
    $xmldoc->save('account.xml');
}
 header('Location: account.xml');
?>