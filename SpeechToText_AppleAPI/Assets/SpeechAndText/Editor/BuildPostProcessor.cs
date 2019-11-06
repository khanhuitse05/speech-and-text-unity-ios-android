//
// Copyright (c) 2017 eppz! mobile, Gergely Borbás (SP)
//
// http://www.twitter.com/_eppz
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;



public class BuildPostProcessor
{


    [PostProcessBuildAttribute(1)]
    public static void OnPostProcessBuild(BuildTarget target, string path)
    {
        if (target == BuildTarget.iOS)
        {
            // Read.
            string projectPath = PBXProject.GetPBXProjectPath(path);
            PBXProject project = new PBXProject();
            project.ReadFromString(File.ReadAllText(projectPath));
            string targetName = PBXProject.GetUnityTargetName();
            string targetGUID = project.TargetGuidByName(targetName);

            AddFrameworks(project, targetGUID);

            // Write.
            File.WriteAllText(projectPath, project.WriteToString());
        }
    }

    static void AddFrameworks(PBXProject project, string targetGUID)
    {
        
        project.AddFrameworkToProject(targetGUID, "speech.framework", false);
        
        // Frameworks (eppz! Photos, Google Analytics).
//        project.AddFrameworkToProject(targetGUID, "MessageUI.framework", false);
//        project.AddFrameworkToProject(targetGUID, "AdSupport.framework", false);
//        project.AddFrameworkToProject(targetGUID, "CoreData.framework", false);
//        project.AddFrameworkToProject(targetGUID, "SystemConfiguration.framework", false);
//        project.AddFrameworkToProject(targetGUID, "libz.dylib", false);
//        project.AddFrameworkToProject(targetGUID, "libsqlite3.tbd", false);

        // Add `-ObjC` to "Other Linker Flags".
        project.AddBuildProperty(targetGUID, "OTHER_LDFLAGS", "-ObjC");
    }
}