$today = Get-Date -Hour 0 -Minute 0 -Second 0 -Format yyyyMMddHHmmss
$yesterday = (Get-Date -Hour 0 -Minute 0 -Second 0).AddDays(-1) 

.\elastic2sql.exe (Get-Date $yesterday -Format yyyyMMddHHmmss) $today 100