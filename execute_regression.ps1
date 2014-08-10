$xmlFilePath = [System.IO.Path]::Combine( $(get-location), "testResults.xml")

"Results redirected to" + $xmlFilePath

if ( Test-Path $xmlFilePath )
{
    Remove-Item $xmlFilePath
}

cd .\fitnesse

java -jar fitnesse-standalone.jar -c "InternalTests.NetRunnerRegression?suite&format=xml" -b $xmlFilePath

cd..

[xml]$xml = Get-Content $xmlFilePath

$rightNode = $xml.selectNodes('//testResults//finalCounts//right')[0]
$wrongNode = $xml.selectNodes('//testResults//finalCounts//wrong')[0]
$exceptionsNode = $xml.selectNodes('//testResults//finalCounts//exceptions')[0]

if ( ( "0" -eq $rightNode.InnerText ) -or ("0" -ne $wrongNode.InnerText) -or ("0" -ne $exceptionsNode.InnerText) )
{
    Write-Error ([System.IO.File]::ReadAllText( $xmlFilePath ) );

    return 1;
}

return 0;

