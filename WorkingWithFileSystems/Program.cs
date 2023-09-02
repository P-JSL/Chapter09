using static System.Console;
using static System.IO.Directory;
using static System.IO.Path;
using static System.Environment;

OutputfileSystemInfo();
static void OutputfileSystemInfo()
{
    WriteLine("{0, -33} {1}", arg0: "Path.PathSeparator", arg1: Path.PathSeparator);
    WriteLine("{0, -33} {1}", arg0: "Path.DirectorySeparatorChar", arg1: Path.DirectorySeparatorChar);
    WriteLine("{0, -33} {1}", arg0: "Directory.GetCurrentDirectory", arg1: Directory.GetCurrentDirectory());
    WriteLine("{0, -33} {1}", arg0: "Environment.CurrentDirectory", arg1: Environment.CurrentDirectory);
    WriteLine("{0, -33} {1}", arg0: "Environment.SystemDirectory", arg1: Environment.SystemDirectory);
    WriteLine("{0, -33} {1}", arg0: "Path.GetTempPath", arg1: Path.GetTempPath());
    WriteLine("GetFolderPath(SpecialFolder)");
    WriteLine("{0, -33} {1}", arg0: ".System", arg1: GetFolderPath(SpecialFolder.System));
    WriteLine("{0, -33} {1}", arg0: ".ApplicationData", arg1: GetFolderPath(SpecialFolder.ApplicationData));
    WriteLine("{0, -33} {1}", arg0: ".MyDocuments", arg1: GetFolderPath(SpecialFolder.MyDocuments));
    WriteLine("{0, -33} {1}", arg0: ".Personal", arg1: GetFolderPath(SpecialFolder.Personal));

}


// 드라이브

WorkWithDrives();
static void WorkWithDrives()
{
    WriteLine("{0,-30}  |   {1,-10} |   {2,-7}  |   {3,18}  |   {4,18}", "NAME", "TYPE", "FORMAT", "SIZE(BYTES)", "FREE SPACE");
    foreach(DriveInfo drive in DriveInfo.GetDrives())
    {
        if (drive.IsReady)
        {
            WriteLine("{0,-30}  |   {1,-10} |   {2,-7}  |   {3,18:N0}  |   {4,18:N0}",
                drive.Name,drive.DriveType,drive.DriveFormat,drive.TotalSize,drive.AvailableFreeSpace);
        }
    }
}

//디렉토리
WorkWithDirectories();
static void WorkWithDirectories()
{
    string newFolder = Combine(
        GetFolderPath(SpecialFolder.Personal), "Code","Chapter09","NewFolder"
        );
    WriteLine($"Working with: {newFolder}");
    //생성폴더 존재 확인
    WriteLine($"Does it exist ? {Exists(newFolder)}");

    //디렉토리 생성
    WriteLine("Creating it...");
    CreateDirectory(newFolder);
    WriteLine($"Does it exist ? {Exists(newFolder)}");
    Write("Confirm the directory exists, and then press ENTER: ");
    ReadLine();
    //삭제
    WriteLine("Deleting it...");
    Delete(newFolder,recursive:true);
}
//파일 다루기
WorkWithFiles();
static void WorkWithFiles()
{
    // 사용자 폴더에 새 디렉토리 경로 지정
    string dir = Combine(
            GetFolderPath(SpecialFolder.Personal), "Code", "Chapter09", "OutputFiles"
        );
    CreateDirectory(dir);

    //파일경로지정
    string textFile = Combine(dir, "Dummy.txt");
    string backupFile = Combine(dir, "Dummy.bak");
    WriteLine($"Working with : {textFile}");
    //파일존재 확인
    WriteLine($"Does it exist? {File.Exists(textFile)}");

    //파일 생성 후, 파일 안에 1줄의 텍스트 입력
    StreamWriter textWriter = File.CreateText(textFile);
    textWriter.WriteLine("Hello, C#!");
    textWriter.Close();
    WriteLine($"Does it exist? {File.Exists(textFile)}");
    //파일 복사
    File.Copy(sourceFileName:textFile,destFileName:backupFile,overwrite:true);
    WriteLine($"Does {backupFile} exist? {File.Exists(backupFile)}");
    Write("Confirm the files exist, and then press ENTER: ");
    ReadLine();
    //파일 삭제
    File.Delete(textFile);
    WriteLine($"Does it exist? {File.Exists(textFile)}");
    //백업한 파일에서 읽기
    WriteLine($"Reading contents of {backupFile}");
    StreamReader textReader = File.OpenText(backupFile);
    WriteLine(textReader.ReadToEnd());
    textReader.Close();


    //경로
    WriteLine($"Folder Name : {GetDirectoryName(textFile)}");
    WriteLine($"File Name : {GetFileName(textFile)}");
    WriteLine($"File Name without Extension {0}" , GetFileNameWithoutExtension(textFile));
    WriteLine($"File Extension : {GetExtension(textFile)}");
    WriteLine($"Random File Name : {GetRandomFileName()}");
    WriteLine($"Temporary File Name : {GetTempFileName()}");

    //파일의 정보 얻기
    FileInfo info = new(backupFile);
    WriteLine($"{backupFile} : ");
    WriteLine($"Contains {info.Length} bytes");
    WriteLine($"Last accessed {info.LastAccessTime}");
    WriteLine($"Has readonly set to {info.IsReadOnly}");
}