using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniversalEditor.DataFormats.Markup.XML;
using UniversalEditor.ObjectModels.Lighting.Fixture;
using UniversalEditor.ObjectModels.Markup;

namespace UniversalEditor.DataFormats.Lighting.Fixture.Auraluminous
{
    public class XMLFixtureDataFormat : XMLDataFormat
    {
        private static DataFormatReference _dfr = null;
        protected override DataFormatReference MakeReferenceInternal()
        {
            if (_dfr == null)
            {
                _dfr = new DataFormatReference(GetType());
                _dfr.Capabilities.Add(typeof(FixtureObjectModel), DataFormatCapabilities.All);
                // _dfr.Filters.Add("Auraluminous fixture", new string[] { "*.alf" });
            }
            return _dfr;
        }

        protected override void BeforeLoadInternal(Stack<ObjectModel> objectModels)
        {
            base.BeforeLoadInternal(objectModels);
            objectModels.Push(new MarkupObjectModel());
        }
        protected override void AfterLoadInternal(Stack<ObjectModel> objectModels)
        {
            base.AfterLoadInternal(objectModels);

            MarkupObjectModel mom = (objectModels.Pop() as MarkupObjectModel);
            FixtureObjectModel fixture = (objectModels.Pop() as FixtureObjectModel);

            MarkupTagElement tagFixtureType = (mom.Elements["AuraluminousFixtureType"] as MarkupTagElement);
            if (tagFixtureType == null) throw new InvalidDataFormatException("File does not contain top-level \"AuraluminousFixtureType\" tag");

            MarkupAttribute attID = tagFixtureType.Attributes["ID"];
            if (attID != null)
            {
                fixture.ID = new Guid(attID.Value);
            }

            MarkupTagElement tagInformation = (tagFixtureType.Elements["Information"] as MarkupTagElement);
            if (tagInformation != null)
            {
                MarkupTagElement tagTitle = (tagInformation.Elements["Title"] as MarkupTagElement);
                if (tagTitle != null) fixture.Model = tagTitle.Value;

                MarkupTagElement tagManufacturer = (tagInformation.Elements["Manufacturer"] as MarkupTagElement);
                if (tagManufacturer != null) fixture.Manufacturer = tagManufacturer.Value;
            }

            #region Channels
            {
                MarkupTagElement tagChannels = (tagFixtureType.Elements["Channels"] as MarkupTagElement);
                if (tagChannels != null)
                {
                    foreach (MarkupElement elChannel in tagChannels.Elements)
                    {
                        MarkupTagElement tagChannel = (elChannel as MarkupTagElement);
                        if (tagChannel == null) continue;
                        if (tagChannel.FullName != "Channel") continue;

                        Channel channel = new Channel();

                        MarkupAttribute attChannelID = tagChannel.Attributes["ID"];
                        if (attChannelID != null) channel.ID = new Guid(attChannelID.Value);

                        MarkupAttribute attTitle = tagChannel.Attributes["Title"];
                        if (attTitle != null) channel.Name = attTitle.Value;

                        fixture.Channels.Add(channel);
                    }
                }
            }
            #endregion
            #region Modes
            {
                MarkupTagElement tagModes = (tagFixtureType.Elements["Modes"] as MarkupTagElement);
                if (tagModes != null)
                {
                    foreach (MarkupElement elMode in tagModes.Elements)
                    {
                        MarkupTagElement tagMode = (elMode as MarkupTagElement);
                        if (tagMode == null) continue;
                        if (tagMode.FullName != "Mode") continue;

                        Mode mode = new Mode();
                        MarkupAttribute attFixtureID = tagMode.Attributes["ID"];
                        if (attFixtureID != null)
                        {
                            mode.ID = new Guid(attFixtureID.Value);
                        }

                        MarkupAttribute attTitle = tagMode.Attributes["Title"];
                        if (attTitle != null)
                        {
                            mode.Name = attTitle.Value;
                        }

                        MarkupTagElement tagChannels = (tagMode.Elements["Channels"] as MarkupTagElement);
                        if (tagChannels != null)
                        {
                            foreach (MarkupElement elChannel in tagChannels.Elements)
                            {
                                MarkupTagElement tagChannel = (elChannel as MarkupTagElement);
                                if (tagChannel == null) continue;

                                ModeChannel channel = new ModeChannel();

                                MarkupAttribute attModeChannelID = tagChannel.Attributes["ID"];
                                if (attModeChannelID != null)
                                {
                                    Guid id = new Guid(attModeChannelID.Value);
                                    channel.Channel = fixture.Channels[id];
                                }

                                MarkupAttribute attRelativeAddress = tagChannel.Attributes["RelativeAddress"];
                                if (attRelativeAddress != null)
                                {
                                    channel.RelativeAddress = Int32.Parse(attRelativeAddress.Value);
                                }

                                mode.Channels.Add(channel);
                            }
                        }

                        fixture.Modes.Add(mode);
                    }
                }
            }
            #endregion
        }
        protected override void BeforeSaveInternal(Stack<ObjectModel> objectModels)
        {
            base.BeforeSaveInternal(objectModels);
        }
    }
}
