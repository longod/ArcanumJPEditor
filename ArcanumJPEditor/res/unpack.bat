@rem unpack arcanum dat files and humble filter translate files
@rem arg1:dest dir arg2:src dir(Arcanum install dir)

@rem unpack arcanum
mkdir "%1\Arcanum"
cd /d "%1\Arcanum"
"%~dp0\dbmaker.exe" -u "%2\arcanum3.dat"
"%~dp0\dbmaker.exe" -u "%2\arcanum4.dat"
"%~dp0\dbmaker.exe" -u "%2\arcanum5.dat"
"%~dp0\dbmaker.exe" -u "%2\arcanum6.dat"
"%~dp0\dbmaker.exe" -u "%2\arcanum7.dat"
"%~dp0\dbmaker.exe" -u "%2\arcanum8.dat"
@rem unpack modules
mkdir "%1\Arcanum\modules\Arcanum"
cd /d "%1\Arcanum\modules\Arcanum"
"%~dp0\dbmaker.exe" -u "%2\modules\Arcanum.dat"
"%~dp0\dbmaker.exe" -u "%2\modules\Arcanum.PATCH0"
"%~dp0\dbmaker.exe" -u "%2\modules\Arcanum.PATCH1"
"%~dp0\dbmaker.exe" -u "%2\modules\Arcanum.PATCH2"
"%~dp0\dbmaker.exe" -u "%2\modules\Arcanum.PATCH3"
"%~dp0\dbmaker.exe" -u "%2\modules\Arcanum.PATCH4"
"%~dp0\dbmaker.exe" -u "%2\modules\Arcanum.PATCH5"
"%~dp0\dbmaker.exe" -u "%2\modules\Arcanum.PATCH6"
@rem delete
rd "%1\Arcanum\art" /s /q
rd "%1\Arcanum\Module template" /s /q
rd "%1\Arcanum\modules\Arcanum\maps" /s /q
rd "%1\Arcanum\modules\Arcanum\scr" /s /q
rd "%1\Arcanum\modules\Arcanum\Slide" /s /q
rd "%1\Arcanum\modules\Arcanum\townmap" /s /q
rd "%1\Arcanum\modules\Arcanum\WorldMap" /s /q
rd "%1\Arcanum\movies" /s /q
rd "%1\Arcanum\Players" /s /q
rd "%1\Arcanum\portrait" /s /q
rd "%1\Arcanum\proto" /s /q
rd "%1\Arcanum\scr" /s /q
rd "%1\Arcanum\sound" /s /q
:FINISH
