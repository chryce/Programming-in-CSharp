$x=123;
$y=456
if ($x -lt $y)
{
  "abc"
}
else
{
  "xyz"
}

for ($i = 1; $i -lt 5; $i++)
{
    $i;
}

$site= Get-IISAppPool

"Ӧ�ó��������"+$site.Count
 $site[0]

 for ($i = 0; $i -lt $site.Count; $i++)
 {
      $site[$i].Name
 }