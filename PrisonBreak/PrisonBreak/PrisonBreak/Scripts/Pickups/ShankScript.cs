using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using PrisonBreak.Components;

namespace PrisonBreak.Scripts.AI
{
    public class ShankScript : Script
    {
        private static Texture2D shankTexture;

        private PlayerScript playerScript;

        public ShankScript(GameObject parent, PlayerScript playerScript)
            : base(parent)
        {
            this.playerScript = playerScript;
        }

        public override void Update()
        {
        }

        public static GameObject CreateShankGO(ContentManager content, GraphicsDevice gd, PlayerScript playerScript)
        {
            if (shankTexture == null)
            {
                shankTexture = content.Load<Texture2D>("Pickups/Shank");
            }

            GameObject shank = new GameObject();

            shank.AddTransform();
            shank.AddStaticSprite(shankTexture);
            shank.AddRenderer(gd, SpriteTransparency.Transparent);
            shank.AddDynamicRigidBody(new Vector2(10f, 12f));
            shank.RigidBody.Body.CollidesWith = CollisionCats.WorldCategory;
            ShankScript script = new ShankScript(shank, playerScript);
            shank.AddScript(new ShankScript (shank, playerScript));
            shank.AddTrigger(new Vector2(10f, 12f));
            shank.Trigger.CollidesWith = CollisionCats.PlayerCategory;
            shank.Trigger.OnEnter += new FarseerPhysics.Dynamics.OnCollisionEventHandler(script.OnEnter);

            return shank;
        }

        private bool OnEnter(FarseerPhysics.Dynamics.Fixture fixtureA, FarseerPhysics.Dynamics.Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            Renderer.IsEnabled = false;
            RigidBody.Body.Enabled = false;
			Trigger.Enabled = false;
			playerScript.HasShank = true;

            return true;
        }

        
    }
}
