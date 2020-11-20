using CefSharp;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Selenium.CefSharp.Driver.InTarget
{
    public class FrameFinder
    {
        public static IFrame FindFrame(IBrowser browser, IFrame parentFrame, List<string> frameNames, int childIndex)
        {
            if (childIndex < 0) return null;
            var children = GetChildren(browser, parentFrame, frameNames);
            if (children.Length <= childIndex) return null;
            return children[childIndex];
        }

        public static IFrame GetMainFrame(IBrowser browser)
        {
            foreach (var e in browser.GetFrameIdentifiers())
            {
                var frame = browser.GetFrame(e);
                if (frame.IsMain) return frame;
            }
            return null;
        }
        static IFrame[] GetChildren(IBrowser browser, IFrame parentFrame, List<string> frameNames)
        {
            var allFrames = new List<IFrame>();
            foreach (var e in browser.GetFrameIdentifiers())
            {
                allFrames.Add(browser.GetFrame(e));
            }

            var children = new List<IFrame>();
            foreach (var frame in allFrames)
            {
                var parent = frame.Parent;
                if (parent == null) continue;

                if (parent.Identifier == parentFrame.Identifier)
                {
                    children.Add(frame);
                }
            }

            //sort
            //For names with names on dom, use the order of appearance.
            children = children.OrderBy(e => e.Name).ToList();
            if (children.Count < frameNames.Count) return children.ToArray();

            var sortedChildren = new IFrame[children.Count];

            for (int i = 0; i < children.Count; i++)
            {
                var e = children[i];
                var index = frameNames.IndexOf(e.Name);
                if (index == -1) continue;
                sortedChildren[index] = e;
                children[i] = null;
            }

            int j = 0;
            for (int i = 0; i < children.Count; i++)
            {
                var e = children[i];
                if (e == null) continue;

                for (; j < sortedChildren.Length; j++)
                {
                    if (sortedChildren[j] == null)
                    {
                        sortedChildren[j] = e;
                        j++;
                        break;
                    }
                }
            }
            return sortedChildren;
        }
    }
}
