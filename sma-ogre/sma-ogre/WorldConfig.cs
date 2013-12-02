using Mogre;
using System;

namespace sma_ogre
{
    class UnkownConfigKeyException : Exception {}
    class UnkownConfigSectionException : Exception {}

    class WorldConfig
    {
        private static WorldConfig singleton;

        private Random rnd;

        private int     mInitialGoodAgentsNumber;
        private int     mInitialBadAgentsNumber;
        private int     mInitialBrickNumber;

        private float   mGroundWidth;
        private float   mGroundLength;
        private float   mGroundBorderWidth;

        private string  mOgreMesh;
        private string  mRobotMesh;
        private string  mBrickMesh;

        private float[] mDefaultSpeedRange;
        private float[] mBuilderSpeedRange;
        private float[] mWreckerSpeedRange;

        private ColourValue mAmbientLightOn;
        private ColourValue mAmbientLightOff;
        private bool        mAmbientLightIsOn;

        private float       mSpeedFactor;
        private bool        mPause;

        private WorldConfig()
        {
            rnd               = new Random();
            mAmbientLightIsOn = true;
            mSpeedFactor      = 1f;
            mPause            = true;

            try
            {
                DefineConfig();
            }
            catch (UnkownConfigKeyException e)
            {
                Console.WriteLine(e);
            }
            catch (UnkownConfigSectionException e)
            {
                Console.WriteLine(e);
            }
        }

        public static WorldConfig Singleton
        {
            get
            {
                if (singleton == null)
                {
                    singleton = new WorldConfig();
                }
                return singleton;
            }
        }

        public int RandInt(int min, int max)
        {
            return rnd.Next(min, max);
        }

        public float RandFloat(float min, float max)
        {
            return (float)rnd.NextDouble() * (max - min) + min;
        }

        // Properties to get configuration

        public float GroundWidth
        {
            get { return mGroundWidth; }
        }

        public float GroundLength
        {
            get { return mGroundLength; }
        }

        public float GroundBorderWidth
        {
            get { return mGroundBorderWidth; }
        }

        public int InitialGoodAgentsNumber
        {
            get { return mInitialGoodAgentsNumber; }
        }

        public int InitialBadAgentsNumber
        {
            get { return mInitialBadAgentsNumber; }
        }

        public int InitialBrickNumber
        {
            get { return mInitialBrickNumber; }
        }

        public ColourValue AmbientLightOn
        {
            get { return mAmbientLightOn; }
        }

        public ColourValue AmbientLightOff
        {
            get { return mAmbientLightOff; }
        }

        public string OgreMesh
        {
            get { return mOgreMesh; }
        }

        public string RobotMesh
        {
            get { return mRobotMesh; }
        }

        public string BrickMesh
        {
            get { return mBrickMesh; }
        }

        public float[] DefaultSpeedRange
        {
            get { return mDefaultSpeedRange; }
        }

        public float[] BuilderSpeedRange
        {
            get { return mBuilderSpeedRange; }
        }

        public float[] WreckerSpeedRange
        {
            get { return mWreckerSpeedRange; }
        }

        public ColourValue SwitchedLight()
        {
            mAmbientLightIsOn = !mAmbientLightIsOn;
            if (mAmbientLightIsOn)
            {
                return mAmbientLightOn;
            }
            else
            {
                return mAmbientLightOff;
            }
        }

        public float SpeedFactor
        {
            get { return mSpeedFactor; }
        }

        public void SpeedFactorDecrease()
        {
            if (mSpeedFactor > 0.01)
            {
                mSpeedFactor /= 2;
            }
        }

        public void SpeedFactorIncrease()
        {
            if (mSpeedFactor < 100)
            {
                mSpeedFactor *= 2;
            }
        }

        public bool Pause
        {
            get { return mPause; }
        }

        public void SwitchPause()
        {
            mPause = !mPause;
        }

        private void DefineConfig()
        {
            ConfigFile cf = new ConfigFile();
            cf.Load("world.cfg", "\t:=", true);
            var section = cf.GetSectionIterator();
            while (section.MoveNext())
            {
                foreach (var line in section.Current)
                {
                    if (section.CurrentKey.Equals("General"))
                    {
                        LoadGeneralConfig(line);
                    }
                    else if (section.CurrentKey.Equals("Ground"))
                    {
                        LoadGroundConfig(line);
                    }
                    else if (section.CurrentKey.Equals("Meshes"))
                    {
                        LoadMeshesConfig(line);
                    }
                    else if (section.CurrentKey.Equals("Behavior"))
                    {
                        LoadBehaviorConfig(line);
                    }
                    else
                    {
                        throw new UnkownConfigSectionException();
                    }
                }
            }
        }

        private void LoadGeneralConfig(System.Collections.Generic.KeyValuePair<string,string> line)
        {
            if (line.Key.Equals("InitialGoodAgentsNumber"))
            {
                mInitialGoodAgentsNumber = int.Parse(line.Value);
            }
            else if (line.Key.Equals("InitialBadAgentsNumber"))
            {
                mInitialBadAgentsNumber = int.Parse(line.Value);
            }
            else if (line.Key.Equals("InitialBrickNumber"))
            {
                mInitialBrickNumber = int.Parse(line.Value);
            }
            else if (line.Key.Equals("AmbientLightOn"))
            {
                string[] lightOnColors = line.Value.Split(',');
                mAmbientLightOn = new ColourValue(FloatParse(lightOnColors[0]),
                                                  FloatParse(lightOnColors[1]),
                                                  FloatParse(lightOnColors[2]));
            }
            else if (line.Key.Equals("AmbientLightOff"))
            {
                string[] lightOffColors = line.Value.Split(',');
                mAmbientLightOff = new ColourValue(FloatParse(lightOffColors[0]),
                                                   FloatParse(lightOffColors[1]),
                                                   FloatParse(lightOffColors[2]));
            }
            else {
                throw new UnkownConfigKeyException();
            }
        }

        private void LoadGroundConfig(System.Collections.Generic.KeyValuePair<string,string> line)
        {
            if (line.Key.Equals("Width"))
            {
                mGroundWidth = int.Parse(line.Value);
            }
            else if (line.Key.Equals("Length"))
            {
                mGroundLength = int.Parse(line.Value);
            }
            else if (line.Key.Equals("BorderWidth"))
            {
                mGroundBorderWidth = int.Parse(line.Value);
            }
            else {
                throw new UnkownConfigKeyException();
            }
        }

        private void LoadMeshesConfig(System.Collections.Generic.KeyValuePair<string,string> line)
        {
            if (line.Key.Equals("Ogre"))
            {
                mOgreMesh = line.Value;
            }
            else if (line.Key.Equals("Robot"))
            {
                mRobotMesh = line.Value;
            }
            else if (line.Key.Equals("Brick"))
            {
                mBrickMesh = line.Value;
            }
            else {
                throw new UnkownConfigKeyException();
            }
        }

        private void LoadBehaviorConfig(System.Collections.Generic.KeyValuePair<string,string> line)
        {
            if (line.Key.Equals("DefaultSpeedRange"))
            {
                string[] defaultSpeedRange = line.Value.Split(',');
                mDefaultSpeedRange    = new float[2];
                mDefaultSpeedRange[0] = FloatParse(defaultSpeedRange[0]);
                mDefaultSpeedRange[1] = FloatParse(defaultSpeedRange[1]);
            }
            else if (line.Key.Equals("BuilderSpeedRange"))
            {
                string[] builderSpeedRange = line.Value.Split(',');
                mBuilderSpeedRange    = new float[2];
                mBuilderSpeedRange[0] = FloatParse(builderSpeedRange[0]);
                mBuilderSpeedRange[1] = FloatParse(builderSpeedRange[1]);
            }
            else if (line.Key.Equals("WreckerSpeedRange"))
            {
                string[] wreckerSpeedRange = line.Value.Split(',');
                mWreckerSpeedRange    = new float[2];
                mWreckerSpeedRange[0] = FloatParse(wreckerSpeedRange[0]);
                mWreckerSpeedRange[1] = FloatParse(wreckerSpeedRange[1]);
            }
            else {
                throw new UnkownConfigKeyException();
            }
        }

        private float FloatParse(string s)
        {
            return float.Parse(s, System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
