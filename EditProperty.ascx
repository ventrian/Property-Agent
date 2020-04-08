<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="EditProperty.ascx.vb" Inherits="Ventrian.PropertyAgent.EditProperty" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="Agent" TagName="Options" Src="Options.ascx" %>
<table cellpadding="0" cellspacing="0" width="100%">
	<tr>
		<td align="left">
			<asp:Repeater ID="rptBreadCrumbs" Runat="server">
				<ItemTemplate><a href='<%# DataBinder.Eval(Container.DataItem, "Url") %>' class="NormalBold"><%# DataBinder.Eval(Container.DataItem, "Caption") %></a></ItemTemplate>
				<SeparatorTemplate>&nbsp;&#187;&nbsp;</SeparatorTemplate>
			</asp:Repeater>
		</td>
		<td align="right">
			<Agent:Options id="Options1" runat="server" />
		</td>
	</tr>
	<tr>
		<td height="5" colspan="2"></td>
	</tr>
</table>

<asp:PlaceHolder ID="phTop" runat="server" />
<asp:Panel id="pnlUpdate" runat="server">
<dnn:sectionhead id="dshPropertyDetails" cssclass="Head" runat="server" text="View Options" section="tblPropertyDetails"
	includerule="True"></dnn:sectionhead>
<TABLE id="tblPropertyDetails" cellSpacing="0" cellPadding="2" width="100%" summary="Property Details Design Table"
	border="0" runat="server">
	<TR>
		<TD colSpan="3">
			<asp:label id="lblPropertyDetailsHelp" cssclass="Normal" runat="server"
				enableviewstate="False"></asp:label></TD>
	</TR>
	<TR>
		<TD width="25"></TD>
		<TD colSpan="2">
			<table id="tblPropertyDetail" cellSpacing="2" cellPadding="2" width="100%" summary="Property Design Table"
				border="0">
				<tr align="top" runat="server" id="trType">
					<td class="SubHead" width="150"><dnn:label id="plType" runat="server" suffix=":" controlname="drpTypes"></dnn:label></td>
					<td>
						<asp:dropdownlist id="drpTypes" DataValueField="PropertyTypeID" DataTextField="NameIndented" Runat="server"></asp:dropdownlist>
						<asp:RequiredFieldValidator ID="valPropertyType" Runat="server" ControlToValidate="drpTypes" CssClass="NormalRed"
							Display="None" InitialValue="-1" SetFocusOnError="True" />
					</td>
				</tr>
				<asp:repeater id="rptDetails" Runat="server">
					<ItemTemplate>
						<tr vAlign="top" runat="server" id="trItem">
							<TD class="SubHead" width="150" valign="middle">
								<label id="label" runat="server">
									<asp:linkbutton id="cmdHelp" tabindex="-1" runat="server" CausesValidation="False" enableviewstate="False">
										<asp:image id="imgHelp" tabindex="-1" runat="server" imageurl="~/images/help.gif" enableviewstate="False"></asp:image>
									</asp:linkbutton>
									<asp:label id="lblLabel" runat="server" enableviewstate="False"></asp:label>
								</label>
								<asp:panel id="pnlHelp" runat="server" cssClass="Help" enableviewstate="False">
									<asp:label id="lblHelp" runat="server" enableviewstate="False"></asp:label>
								</asp:panel>
							</TD>
							<TD align="left">
								<asp:PlaceHolder ID="phValue" Runat="server" />
							</TD>
						</tr>
						<tr valign="top" runat="server" id="trLabel" visible="false">
						    <td colspan="2">
								<asp:PlaceHolder ID="phLabel" Runat="server" />
						    </td>
						</tr>
					</ItemTemplate>
				</asp:repeater>
			</table>
		</TD>
	</TR>
</TABLE>
</asp:Panel>

<asp:PlaceHolder ID="phLocation" runat="server">
<dnn:sectionhead id="dshLocation" cssclass="Head" runat="server" text="Location Details" section="tblLocationDetails"
	includerule="True"></dnn:sectionhead>
<table id="tblLocationDetails" cellspacing="0" cellpadding="2" width="100%" summary="Property Details Design Table"
	border="0" runat="server">
<tr>
    <td colspan="3"><asp:label id="lblLocationDetails" ResourceKey="LocationDetails" cssclass="Normal" runat="server" enableviewstate="False"></asp:label></td>
</tr>
<tr>
	<td width="25"></td>
	<td colSpan="2">
	    <table id="tblLocationDetail" cellSpacing="2" cellPadding="2" width="100%" summary="Location Detail Table" border="0">
		<tr valign="middle">
			<td class="SubHead" width="150"><dnn:label id="FindAddress" runat="server" controlname="txtAddress" suffix=":"></dnn:label></td>
			<td>
			    <asp:TextBox ID="txtAddress" runat="server" onkeydown="if(event.keyCode==13) {showAddress(this.value);}" Width="300px" />
			    <input type="button" value="Find Address!" id="findaddressbutton" />
			</td>
		</tr>
		<tr valign="middle">
			<td class="SubHead" width="150"><dnn:label id="lblLatitude" runat="server" controlname="txtLatitude" suffix=":"></dnn:label></td>
			<td>
			    <asp:TextBox ID="txtLatitude" runat="server" Width="100px" />
			</td>
		</tr>
		<tr valign="middle">
			<td class="SubHead" width="150"><dnn:label id="lblLongitude" runat="server" controlname="txtLongitude" suffix=":"></dnn:label></td>
			<td>
			    <asp:TextBox ID="txtLongitude" runat="server" Width="100px" />
			</td>
		</tr>
		<tr>
		    <td colspan="2">
                <script async defer src="<%= GetMapUrl() %>" type="text/javascript"></script>
<asp:literal id="litMapCenter" runat="server" />
       <div align="center" id="map" style="width: 100%; height: 400px"></div>         
                		    
               
			   <asp:placeholder id="phMapLoad" runat="server">
			   </asp:placeholder>
			    <script type="text/javascript">

                    function load() {
					
                        // The location of Uluru
                        var uluru = { lat: parseFloat(document.getElementById("<%= txtLatitude.ClientID %>").value), lng: parseFloat(document.getElementById("<%= txtLongitude.ClientID %>").value) };
                        
                        // The map, centered at Uluru
                        var map = new google.maps.Map(
                            document.getElementById('map'), { zoom: 18, center: uluru });

                        // The marker, positioned at Uluru
                        var marker = new google.maps.Marker({
                            position: uluru,
                            map: map,
                            title: 'Client Address',
                            draggable: true
                        });


                        var geocoder = new google.maps.Geocoder();

                        google.maps.event.addListener(marker, 'dragend', function (marker) {
                            var latLng = marker.latLng;
                       
                            document.getElementById("<%= txtLatitude.ClientID %>").value = latLng.lat();
                            document.getElementById("<%= txtLongitude.ClientID %>").value = latLng.lng();


                        });

                        

                        document.getElementById('findaddressbutton').addEventListener('click', function () {
                            geocodeAddress(geocoder, map, marker);
                        });

                    }

                    function geocodeAddress(geocoder, resultsMap, marker) {
                        var address = document.getElementById('<%= txtAddress.ClientID %>').value;
                        geocoder.geocode({ 'address': address }, function (results, status) {
                            if (status === 'OK') {
                                resultsMap.setCenter(results[0].geometry.location);

                                var latitude = results[0].geometry.location.lat();
                                var longitude = results[0].geometry.location.lng();

                                document.getElementById("<%= txtLatitude.ClientID %>").value = latitude;
                                document.getElementById("<%= txtLongitude.ClientID %>").value = longitude;
                               // alert(latitude);
                                marker.setPosition(results[0].geometry.location);
                                map.panTo(new google.maps.LatLng(latitude, longitude));
                                
                               // var marker = new google.maps.Marker({map: resultsMap, position: results[0].geometry.location});
                            } else {
                                alert('Geocode was not successful for the following reason: ' + status);
                            }
                        });
                    }


     </script>
			   
			   
		        </td>
		    </tr>
		</table>
	</td>
</tr>
</table>	
</asp:PlaceHolder>
<asp:PlaceHolder ID="phPublishingDetails" Runat="server">
<dnn:sectionhead id="dshPublishDetails" cssclass="Head" runat="server" text="Publishing Details"
	section="tblPublishDetails" resourcekey="PublishDetails" includerule="True"></dnn:sectionhead>
<TABLE id="tblPublishDetails" cellSpacing="0" cellPadding="2" width="100%" summary="Publishing Details Design Table"
	border="0" runat="server">
	<TR>
		<TD colSpan="3">
			<asp:label id="lblPublishDetails" cssclass="Normal" runat="server" 
				enableviewstate="False"></asp:label></TD>
	</TR>
	<TR>
		<TD width="25"></TD>
		<TD colSpan="2">
			<table id="tblPublishDetail" cellSpacing="2" cellPadding="2" width="100%" summary="Publish Design Table"
				border="0">
				<tr vAlign="top" id="trOwner" runat="server">
					<td class="SubHead" width="150"><dnn:label id="plOwner" runat="server" controlname="lstOwner" suffix=":"></dnn:label></td>
					<td>
						<asp:dropdownlist id="drpOwner" runat="server" Width="300px" Visible="False" DataTextField="DisplayName" DataValueField="UserID"></asp:dropdownlist>
						<asp:label id="lblOwner" runat="server" CssClass="NormalBold"></asp:label><br>
						<asp:linkbutton id="cmdChange" runat="server" cssclass="CommandButton"
							causesvalidation="False" text="Change Owner" borderstyle="none">Change Owner</asp:linkbutton></td>
				</tr>
				<tr align="top">
					<td class="SubHead" width="150"><dnn:label id="plCreationDate" runat="server" resourcekey="CreationDate" suffix=":" controlname="txtCreationDate"></dnn:label></td>
					<td>
						<asp:DropDownList ID="drpCreationTimeHour" Runat="server">
							<asp:ListItem Value="0">0</asp:ListItem>
							<asp:ListItem Value="1">1</asp:ListItem>
							<asp:ListItem Value="2">2</asp:ListItem>
							<asp:ListItem Value="3">3</asp:ListItem>
							<asp:ListItem Value="4">4</asp:ListItem>
							<asp:ListItem Value="5">5</asp:ListItem>
							<asp:ListItem Value="6">6</asp:ListItem>
							<asp:ListItem Value="7">7</asp:ListItem>
							<asp:ListItem Value="8">8</asp:ListItem>
							<asp:ListItem Value="9">9</asp:ListItem>
							<asp:ListItem Value="10">10</asp:ListItem>
							<asp:ListItem Value="11">11</asp:ListItem>
							<asp:ListItem Value="12">12</asp:ListItem>
							<asp:ListItem Value="13">13</asp:ListItem>
							<asp:ListItem Value="14">14</asp:ListItem>
							<asp:ListItem Value="15">15</asp:ListItem>
							<asp:ListItem Value="16">16</asp:ListItem>
							<asp:ListItem Value="17">17</asp:ListItem>
							<asp:ListItem Value="18">18</asp:ListItem>
							<asp:ListItem Value="19">19</asp:ListItem>
							<asp:ListItem Value="20">20</asp:ListItem>
							<asp:ListItem Value="21">21</asp:ListItem>
							<asp:ListItem Value="22">22</asp:ListItem>
							<asp:ListItem Value="23">23</asp:ListItem>
						</asp:DropDownList>
						:
						<asp:DropDownList ID="drpCreationTimeMinute" Runat="server">
							<asp:ListItem Value="0">00</asp:ListItem>
							<asp:ListItem Value="1">01</asp:ListItem>
							<asp:ListItem Value="2">02</asp:ListItem>
							<asp:ListItem Value="3">03</asp:ListItem>
							<asp:ListItem Value="4">04</asp:ListItem>
							<asp:ListItem Value="5">05</asp:ListItem>
							<asp:ListItem Value="6">06</asp:ListItem>
							<asp:ListItem Value="7">07</asp:ListItem>
							<asp:ListItem Value="8">08</asp:ListItem>
							<asp:ListItem Value="9">09</asp:ListItem>
							<asp:ListItem Value="10">10</asp:ListItem>
							<asp:ListItem Value="11">11</asp:ListItem>
							<asp:ListItem Value="12">12</asp:ListItem>
							<asp:ListItem Value="13">13</asp:ListItem>
							<asp:ListItem Value="14">14</asp:ListItem>
							<asp:ListItem Value="15">15</asp:ListItem>
							<asp:ListItem Value="16">16</asp:ListItem>
							<asp:ListItem Value="17">17</asp:ListItem>
							<asp:ListItem Value="18">18</asp:ListItem>
							<asp:ListItem Value="19">19</asp:ListItem>
							<asp:ListItem Value="20">20</asp:ListItem>
							<asp:ListItem Value="21">21</asp:ListItem>
							<asp:ListItem Value="22">22</asp:ListItem>
							<asp:ListItem Value="23">23</asp:ListItem>
							<asp:ListItem Value="24">24</asp:ListItem>
							<asp:ListItem Value="25">25</asp:ListItem>
							<asp:ListItem Value="26">26</asp:ListItem>
							<asp:ListItem Value="27">27</asp:ListItem>
							<asp:ListItem Value="28">28</asp:ListItem>
							<asp:ListItem Value="29">29</asp:ListItem>
							<asp:ListItem Value="30">30</asp:ListItem>
							<asp:ListItem Value="31">31</asp:ListItem>
							<asp:ListItem Value="32">32</asp:ListItem>
							<asp:ListItem Value="33">33</asp:ListItem>
							<asp:ListItem Value="34">34</asp:ListItem>
							<asp:ListItem Value="35">35</asp:ListItem>
							<asp:ListItem Value="36">36</asp:ListItem>
							<asp:ListItem Value="37">37</asp:ListItem>
							<asp:ListItem Value="38">38</asp:ListItem>
							<asp:ListItem Value="39">39</asp:ListItem>
							<asp:ListItem Value="40">40</asp:ListItem>
							<asp:ListItem Value="41">41</asp:ListItem>
							<asp:ListItem Value="42">42</asp:ListItem>
							<asp:ListItem Value="43">43</asp:ListItem>
							<asp:ListItem Value="44">44</asp:ListItem>
							<asp:ListItem Value="45">45</asp:ListItem>
							<asp:ListItem Value="46">46</asp:ListItem>
							<asp:ListItem Value="47">47</asp:ListItem>
							<asp:ListItem Value="48">48</asp:ListItem>
							<asp:ListItem Value="49">49</asp:ListItem>
							<asp:ListItem Value="50">50</asp:ListItem>
							<asp:ListItem Value="51">51</asp:ListItem>
							<asp:ListItem Value="52">52</asp:ListItem>
							<asp:ListItem Value="53">53</asp:ListItem>
							<asp:ListItem Value="54">54</asp:ListItem>
							<asp:ListItem Value="55">55</asp:ListItem>
							<asp:ListItem Value="56">56</asp:ListItem>
							<asp:ListItem Value="57">57</asp:ListItem>
							<asp:ListItem Value="58">58</asp:ListItem>
							<asp:ListItem Value="59">59</asp:ListItem>
						</asp:DropDownList>
						<asp:textbox id="txtCreationDate" cssclass="NormalTextBox" runat="server" width="150" maxlength="15"></asp:textbox>
						<asp:hyperlink id="cmdCreationDate" cssclass="CommandButton" runat="server" resourcekey="Calendar">Calendar</asp:hyperlink>
						<asp:requiredfieldvalidator id="valCreationDateRequired" runat="server" cssclass="NormalRed" resourcekey="valCreationDateRequired.ErrorMessage"
							display="None" errormessage="Creation Date Is Required" controltovalidate="txtCreationDate" SetFocusOnError="True"></asp:requiredfieldvalidator>
						<asp:comparevalidator id="valCreationDate" cssclass="NormalRed" runat="server" controltovalidate="txtCreationDate"
							errormessage="Invalid creation date!" operator="DataTypeCheck" type="Date" display="None" ResourceKey="valCreationDate.ErrorMessage" SetFocusOnError="True"></asp:comparevalidator>
					</td>
				</tr>
				<tr align="top">
					<td class="SubHead" width="150"><dnn:label id="plPublishDate" runat="server" resourcekey="PublishDate" suffix=":" controlname="txtPublishDate"></dnn:label></td>
					<td>
						<asp:DropDownList ID="drpPublishTimeHour" Runat="server">
							<asp:ListItem Value="-">--</asp:ListItem>
							<asp:ListItem Value="0">0</asp:ListItem>
							<asp:ListItem Value="1">1</asp:ListItem>
							<asp:ListItem Value="2">2</asp:ListItem>
							<asp:ListItem Value="3">3</asp:ListItem>
							<asp:ListItem Value="4">4</asp:ListItem>
							<asp:ListItem Value="5">5</asp:ListItem>
							<asp:ListItem Value="6">6</asp:ListItem>
							<asp:ListItem Value="7">7</asp:ListItem>
							<asp:ListItem Value="8">8</asp:ListItem>
							<asp:ListItem Value="9">9</asp:ListItem>
							<asp:ListItem Value="10">10</asp:ListItem>
							<asp:ListItem Value="11">11</asp:ListItem>
							<asp:ListItem Value="12">12</asp:ListItem>
							<asp:ListItem Value="13">13</asp:ListItem>
							<asp:ListItem Value="14">14</asp:ListItem>
							<asp:ListItem Value="15">15</asp:ListItem>
							<asp:ListItem Value="16">16</asp:ListItem>
							<asp:ListItem Value="17">17</asp:ListItem>
							<asp:ListItem Value="18">18</asp:ListItem>
							<asp:ListItem Value="19">19</asp:ListItem>
							<asp:ListItem Value="20">20</asp:ListItem>
							<asp:ListItem Value="21">21</asp:ListItem>
							<asp:ListItem Value="22">22</asp:ListItem>
							<asp:ListItem Value="23">23</asp:ListItem>
						</asp:DropDownList>
						:
						<asp:DropDownList ID="drpPublishTimeMinute" Runat="server">
							<asp:ListItem Value="-">--</asp:ListItem>
							<asp:ListItem Value="0">00</asp:ListItem>
							<asp:ListItem Value="1">01</asp:ListItem>
							<asp:ListItem Value="2">02</asp:ListItem>
							<asp:ListItem Value="3">03</asp:ListItem>
							<asp:ListItem Value="4">04</asp:ListItem>
							<asp:ListItem Value="5">05</asp:ListItem>
							<asp:ListItem Value="6">06</asp:ListItem>
							<asp:ListItem Value="7">07</asp:ListItem>
							<asp:ListItem Value="8">08</asp:ListItem>
							<asp:ListItem Value="9">09</asp:ListItem>
							<asp:ListItem Value="10">10</asp:ListItem>
							<asp:ListItem Value="11">11</asp:ListItem>
							<asp:ListItem Value="12">12</asp:ListItem>
							<asp:ListItem Value="13">13</asp:ListItem>
							<asp:ListItem Value="14">14</asp:ListItem>
							<asp:ListItem Value="15">15</asp:ListItem>
							<asp:ListItem Value="16">16</asp:ListItem>
							<asp:ListItem Value="17">17</asp:ListItem>
							<asp:ListItem Value="18">18</asp:ListItem>
							<asp:ListItem Value="19">19</asp:ListItem>
							<asp:ListItem Value="20">20</asp:ListItem>
							<asp:ListItem Value="21">21</asp:ListItem>
							<asp:ListItem Value="22">22</asp:ListItem>
							<asp:ListItem Value="23">23</asp:ListItem>
							<asp:ListItem Value="24">24</asp:ListItem>
							<asp:ListItem Value="25">25</asp:ListItem>
							<asp:ListItem Value="26">26</asp:ListItem>
							<asp:ListItem Value="27">27</asp:ListItem>
							<asp:ListItem Value="28">28</asp:ListItem>
							<asp:ListItem Value="29">29</asp:ListItem>
							<asp:ListItem Value="30">30</asp:ListItem>
							<asp:ListItem Value="31">31</asp:ListItem>
							<asp:ListItem Value="32">32</asp:ListItem>
							<asp:ListItem Value="33">33</asp:ListItem>
							<asp:ListItem Value="34">34</asp:ListItem>
							<asp:ListItem Value="35">35</asp:ListItem>
							<asp:ListItem Value="36">36</asp:ListItem>
							<asp:ListItem Value="37">37</asp:ListItem>
							<asp:ListItem Value="38">38</asp:ListItem>
							<asp:ListItem Value="39">39</asp:ListItem>
							<asp:ListItem Value="40">40</asp:ListItem>
							<asp:ListItem Value="41">41</asp:ListItem>
							<asp:ListItem Value="42">42</asp:ListItem>
							<asp:ListItem Value="43">43</asp:ListItem>
							<asp:ListItem Value="44">44</asp:ListItem>
							<asp:ListItem Value="45">45</asp:ListItem>
							<asp:ListItem Value="46">46</asp:ListItem>
							<asp:ListItem Value="47">47</asp:ListItem>
							<asp:ListItem Value="48">48</asp:ListItem>
							<asp:ListItem Value="49">49</asp:ListItem>
							<asp:ListItem Value="50">50</asp:ListItem>
							<asp:ListItem Value="51">51</asp:ListItem>
							<asp:ListItem Value="52">52</asp:ListItem>
							<asp:ListItem Value="53">53</asp:ListItem>
							<asp:ListItem Value="54">54</asp:ListItem>
							<asp:ListItem Value="55">55</asp:ListItem>
							<asp:ListItem Value="56">56</asp:ListItem>
							<asp:ListItem Value="57">57</asp:ListItem>
							<asp:ListItem Value="58">58</asp:ListItem>
							<asp:ListItem Value="59">59</asp:ListItem>
						</asp:DropDownList>
						<asp:textbox id="txtPublishDate" cssclass="NormalTextBox" runat="server" width="150" maxlength="15"></asp:textbox>
						<asp:hyperlink id="cmdPublishDate" cssclass="CommandButton" runat="server" resourcekey="Calendar">Calendar</asp:hyperlink>
						<asp:comparevalidator id="valPublishDate" cssclass="NormalRed" runat="server" controltovalidate="txtPublishDate"
							errormessage="<br>Invalid publish date!" operator="DataTypeCheck" type="Date" display="None" ResourceKey="valPublishDate.ErrorMessage" SetFocusOnError="True"></asp:comparevalidator>
					</td>
				</tr>
				<tr align="top">
					<td class="SubHead" width="150"><dnn:label id="plExpiryDate" runat="server" resourcekey="ExpiryDate" suffix=":" controlname="txtExpiryDate"></dnn:label></td>
					<td>
						<asp:DropDownList ID="drpExpiryTimeHour" Runat="server">
							<asp:ListItem Value="-">--</asp:ListItem>
							<asp:ListItem Value="0">0</asp:ListItem>
							<asp:ListItem Value="1">1</asp:ListItem>
							<asp:ListItem Value="2">2</asp:ListItem>
							<asp:ListItem Value="3">3</asp:ListItem>
							<asp:ListItem Value="4">4</asp:ListItem>
							<asp:ListItem Value="5">5</asp:ListItem>
							<asp:ListItem Value="6">6</asp:ListItem>
							<asp:ListItem Value="7">7</asp:ListItem>
							<asp:ListItem Value="8">8</asp:ListItem>
							<asp:ListItem Value="9">9</asp:ListItem>
							<asp:ListItem Value="10">10</asp:ListItem>
							<asp:ListItem Value="11">11</asp:ListItem>
							<asp:ListItem Value="12">12</asp:ListItem>
							<asp:ListItem Value="13">13</asp:ListItem>
							<asp:ListItem Value="14">14</asp:ListItem>
							<asp:ListItem Value="15">15</asp:ListItem>
							<asp:ListItem Value="16">16</asp:ListItem>
							<asp:ListItem Value="17">17</asp:ListItem>
							<asp:ListItem Value="18">18</asp:ListItem>
							<asp:ListItem Value="19">19</asp:ListItem>
							<asp:ListItem Value="20">20</asp:ListItem>
							<asp:ListItem Value="21">21</asp:ListItem>
							<asp:ListItem Value="22">22</asp:ListItem>
							<asp:ListItem Value="23">23</asp:ListItem>
						</asp:DropDownList>
						:
						<asp:DropDownList ID="drpExpiryTimeMinute" Runat="server">
							<asp:ListItem Value="-">--</asp:ListItem>
							<asp:ListItem Value="0">00</asp:ListItem>
							<asp:ListItem Value="1">01</asp:ListItem>
							<asp:ListItem Value="2">02</asp:ListItem>
							<asp:ListItem Value="3">03</asp:ListItem>
							<asp:ListItem Value="4">04</asp:ListItem>
							<asp:ListItem Value="5">05</asp:ListItem>
							<asp:ListItem Value="6">06</asp:ListItem>
							<asp:ListItem Value="7">07</asp:ListItem>
							<asp:ListItem Value="8">08</asp:ListItem>
							<asp:ListItem Value="9">09</asp:ListItem>
							<asp:ListItem Value="10">10</asp:ListItem>
							<asp:ListItem Value="11">11</asp:ListItem>
							<asp:ListItem Value="12">12</asp:ListItem>
							<asp:ListItem Value="13">13</asp:ListItem>
							<asp:ListItem Value="14">14</asp:ListItem>
							<asp:ListItem Value="15">15</asp:ListItem>
							<asp:ListItem Value="16">16</asp:ListItem>
							<asp:ListItem Value="17">17</asp:ListItem>
							<asp:ListItem Value="18">18</asp:ListItem>
							<asp:ListItem Value="19">19</asp:ListItem>
							<asp:ListItem Value="20">20</asp:ListItem>
							<asp:ListItem Value="21">21</asp:ListItem>
							<asp:ListItem Value="22">22</asp:ListItem>
							<asp:ListItem Value="23">23</asp:ListItem>
							<asp:ListItem Value="24">24</asp:ListItem>
							<asp:ListItem Value="25">25</asp:ListItem>
							<asp:ListItem Value="26">26</asp:ListItem>
							<asp:ListItem Value="27">27</asp:ListItem>
							<asp:ListItem Value="28">28</asp:ListItem>
							<asp:ListItem Value="29">29</asp:ListItem>
							<asp:ListItem Value="30">30</asp:ListItem>
							<asp:ListItem Value="31">31</asp:ListItem>
							<asp:ListItem Value="32">32</asp:ListItem>
							<asp:ListItem Value="33">33</asp:ListItem>
							<asp:ListItem Value="34">34</asp:ListItem>
							<asp:ListItem Value="35">35</asp:ListItem>
							<asp:ListItem Value="36">36</asp:ListItem>
							<asp:ListItem Value="37">37</asp:ListItem>
							<asp:ListItem Value="38">38</asp:ListItem>
							<asp:ListItem Value="39">39</asp:ListItem>
							<asp:ListItem Value="40">40</asp:ListItem>
							<asp:ListItem Value="41">41</asp:ListItem>
							<asp:ListItem Value="42">42</asp:ListItem>
							<asp:ListItem Value="43">43</asp:ListItem>
							<asp:ListItem Value="44">44</asp:ListItem>
							<asp:ListItem Value="45">45</asp:ListItem>
							<asp:ListItem Value="46">46</asp:ListItem>
							<asp:ListItem Value="47">47</asp:ListItem>
							<asp:ListItem Value="48">48</asp:ListItem>
							<asp:ListItem Value="49">49</asp:ListItem>
							<asp:ListItem Value="50">50</asp:ListItem>
							<asp:ListItem Value="51">51</asp:ListItem>
							<asp:ListItem Value="52">52</asp:ListItem>
							<asp:ListItem Value="53">53</asp:ListItem>
							<asp:ListItem Value="54">54</asp:ListItem>
							<asp:ListItem Value="55">55</asp:ListItem>
							<asp:ListItem Value="56">56</asp:ListItem>
							<asp:ListItem Value="57">57</asp:ListItem>
							<asp:ListItem Value="58">58</asp:ListItem>
							<asp:ListItem Value="59">59</asp:ListItem>
						</asp:DropDownList>
						<asp:textbox id="txtExpiryDate" cssclass="NormalTextBox" runat="server" width="150" maxlength="15"></asp:textbox>
						<asp:hyperlink id="cmdExpiryDate" cssclass="CommandButton" runat="server" resourcekey="Calendar">Calendar</asp:hyperlink>
						<asp:comparevalidator id="valExpiryDate" cssclass="NormalRed" runat="server" controltovalidate="txtExpiryDate"
							errormessage="Invalid Expiry Date!" operator="DataTypeCheck" type="Date" display="None" ResourceKey="valExpiryDate.ErrorMessage" SetFocusOnError="True"></asp:comparevalidator>
					</td>
				</tr>
				<tr align="top" runat="server" id="trFeatured">
					<td class="SubHead" width="150"><dnn:label id="plFeatured" runat="server" resourcekey="Featured" suffix=":" controlname="chkFeatured"></dnn:label></td>
					<td><asp:checkbox id="chkFeatured" Runat="server" Checked="False"></asp:checkbox></td>
				</tr>
				<tr align="top">
					<td class="SubHead" width="150"><dnn:label id="plApproved" runat="server" resourcekey="Approved" suffix=":" controlname="chkApproved"></dnn:label></td>
					<td><asp:checkbox id="chkApproved" Runat="server" Checked="False"></asp:checkbox></td>
				</tr>
				<tr align="top">
					<td class="SubHead" width="150"><dnn:label id="plPublished" runat="server" resourcekey="plPublished" suffix=":" controlname="chkPublished"></dnn:label></td>
					<td><asp:checkbox id="chkPublished" Runat="server" Checked="False"></asp:checkbox></td>
				</tr>
                <tr align="top">
					<td class="SubHead OnlyForAuthenticated" width="150"><dnn:label id="plOnlyForAuthenticated" runat="server" resourcekey="plOnlyForAuthenticated" suffix=":" controlname="chkOnlyForAuthenticated"></dnn:label></td>
					<td class="OnlyForAuthenticated"><asp:checkbox id="chkOnlyForAuthenticated" Runat="server" Checked="True"></asp:checkbox></td>
				</tr>
			</table>
		</TD>
	</TR>
</TABLE>
<br />
</asp:PlaceHolder>
<asp:PlaceHolder ID="phAuthorDetails" Runat="server">
<dnn:sectionhead id="dshAuthorDetails" cssclass="Head" runat="server" text="Author Details"
	section="tblAuthorDetails" resourcekey="AuthorDetails" includerule="True"></dnn:sectionhead>
<TABLE id="tblAuthorDetails" cellSpacing="0" cellPadding="2" width="100%" summary="Author Details Design Table"
	border="0" runat="server">
	<TR>
		<TD colSpan="3">
			<asp:label id="lblAuthorDetails" cssclass="Normal" runat="server"
				enableviewstate="False"></asp:label></TD>
	</TR>
	<tr align="top">
		<TD width="25"></TD>
		<TD colSpan="2">
			<table id="tblAuthorDetail" cellSpacing="2" cellPadding="2" width="100%" summary="Publish Design Table"
				border="0">
				<tr align="top">
					<td class="SubHead" width="150"><dnn:label id="plUsername" runat="server" resourcekey="Username" suffix=":" controlname="lblUsername"></dnn:label></td>
					<td><asp:Label ID="lblUsername" Runat="server" CssClass="Normal" /></td>
				</tr>
				<tr align="top">
					<td class="SubHead" width="150"><dnn:label id="plDisplayName" runat="server" resourcekey="DisplayName" suffix=":" controlname="lblDisplayName"></dnn:label></td>
					<td><asp:Label ID="lblDisplayName" Runat="server" CssClass="Normal" /></td>
				</tr>
				<tr align="top">
					<td class="SubHead" width="150"><dnn:label id="plEmail" runat="server" resourcekey="Email" suffix=":" controlname="lblEmail"></dnn:label></td>
					<td><asp:Label ID="lblEmail" Runat="server" CssClass="Normal" /></td>
				</tr>
			</table>
		</td>
	</tr>
</table>
<br />
</asp:PlaceHolder>
<asp:PlaceHolder ID="phBottom" runat="server" />
<p align="center">
	<asp:linkbutton id="cmdUpdate" BorderStyle="none" Text="Update" CssClass="CommandButton" runat="server"
		resourcekey="cmdUpdate"></asp:linkbutton>&nbsp;
	<asp:linkbutton id="cmdUpdateEditPhotos" BorderStyle="none" Text="Update &amp; Edit Photos" CssClass="CommandButton"
		runat="server" resourcekey="cmdUpdateEditPhotos"></asp:linkbutton>&nbsp;
	<asp:linkbutton id="cmdClone" BorderStyle="none" Text="Clone" CssClass="CommandButton" runat="server"
		resourcekey="cmdClone"></asp:linkbutton>&nbsp;
	<asp:linkbutton id="cmdCancel" BorderStyle="none" Text="Cancel" CssClass="CommandButton" runat="server"
		resourcekey="cmdCancel" CausesValidation="False"></asp:linkbutton>&nbsp;
	<asp:linkbutton id="cmdDelete" BorderStyle="none" Text="Delete" CssClass="CommandButton" runat="server"
		resourcekey="cmdDelete" CausesValidation="False"></asp:linkbutton>
</p>
<asp:CustomValidator id="valPropertyTypeSubmission" runat="server" CssClass="NormalRed"
							ErrorMessage="You cannot add a property to this property type." Display="Dynamic" />
<asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False" />
