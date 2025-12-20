Perfecto! Mira, ya tengo al 100% mi capa IAM y Shared, PERO AHORA ANALICEMOS Y ESTUDIEMOS SUPER BIEN, para estas fuentes:



Fuentes propuestas: • Offshore Leaks Database: https://offshoreleaks.icij.org o Atributos: Entity, Jurisdiction, Linked To, Data From • The World Bank: https://projects.worldbank.org/en/projectsoperations/procurement/debarred-firms o Atributos: Firm Name, Address, Country, From Date (Ineligibility Period), To Date (Ineligibility Period), Grounds • OFAC: https://sanctionssearch.ofac.treas.gov/ o Atributos: Name, Address, Type, Program(s), List, Score






Por qué? ANALICEMOS ENFOCADO SOLO AL WEB SCRAPPING! Yo te explicaré COMO SE BUSCA y como se obtiene LA DATA pues de CADA UNA DE LAS FUENTES!


------------------------------------------------------------------------------------------------------------------------------------------------
Primero vayamos para:

https://offshoreleaks.icij.org/

Apenas entro a la página, ESPERAS A QUE CARGUE COMPLETAMENTE TODO LO QUE TENGA QUE CARGA.


DIRECTAMENTE YA IRÉ A ESTE LINK:

https://offshoreleaks.icij.org/search?q=British+Virgin+Islands&c=&j=&d=



En ese caso pues simplemente se reemplaza la query o parámetro "q" por el nombre de la empresa y ya se busca, también se puede buscar así:



https://offshoreleaks.icij.org/search?q=%22British+Virgin+Islands%22&c=&j=&d=



O etc, pues se busca y ya.





APENAS ENTRO A LA PÁGINA, LO PRIMERO QUE APARECE, o posblemente aparezca es,:



<div tabindex="-1" class="modal-content p-sm-5 p-2" id="__BVID__73___BV_modal_content_"><header class="modal-header pb-0 border-bottom-0" id="__BVID__73___BV_modal_header_"><h5 class="modal-title" id="__BVID__73___BV_modal_title_"><h2>Please read the statement below before searching</h2></h5><!----></header><div class="modal-body pt-0" id="__BVID__73___BV_modal_body_"><p>There are legitimate uses for offshore companies and trusts. The inclusion of a person or entity in the ICIJ Offshore Leaks Database is not intended to suggest or imply that they have engaged in illegal or improper conduct. Many people and entities have the same or similar names. We suggest you confirm the identities of any individuals or entities included in the database based on addresses or other identifiable information. The data comes directly from the leaked files ICIJ has received in connection with various investigations and each dataset encompasses a defined time period specified in the database. Some information may have changed over time. <a href="/tips/new" class="font-weight-bold">Please contact us</a> if you find an error in the database.</p><form><div class="d-flex flex-column flex-sm-row"><div class="card py-3 py-sm-0 px-3 bg-light flex-grow-1 d-flex flex-column justify-content-center"><input type="hidden" name="accept" value="0"><label for="accept" class="m-0"><input type="checkbox" name="accept" id="accept"><span title="Accept terms"> I have read and understood the terms </span></label></div><div class="flex-auto my-3 my-sm-0 ml-sm-3"><button type="submit" disabled="disabled" class="btn btn-primary btn-block btn-lg"> Submit </button></div></div><p class="insist alert alert-warning mt-4" style="display: none;"> Please accept the terms to continue </p></form></div><!----></div>



Básicamente un modal que me pues se tiene que marcar en "I have read and understood the terms" Y LUEGO dar "Submit".





SOLO SI APARECE ESO CUANDO hagas el web scrapping, pues tienes que hacer eso y dar submit y ya podrás pues estar en la página tranquilo.



Si no aparece, pues seguimos, así que una vez ya accedimos a:



https://offshoreleaks.icij.org/search?q=%22British+Virgin+Islands%22&c=&j=&d=





Ahora, cuando yo entro a esa págnia, qué es lo que sucede EN LA PÁGINA pues ya tengo la tabla con todas las respuestas.



Hice esto en la consola del navegavor:



document.querySelectorAll(".table-responsive")



Y solo me retorna:

NodeList [div.table-responsive]

0: div.table-responsive

length: 1



Y pues si, la única tabla que HAY ES JUSTO LA QUE MUESTRA TODA LA DATA , la tabla tiene este formato: (LO PRIMERO QUE PEGUÉ EN EL CHAT).


<div class="table-responsive">
  <table class="table table-sm table-striped search__results__table">
    <thead class="search__results__table__head thead-light">
      <tr>
        <th class="text-nowrap">
          Entity
        </th>
          <th class="jurisdiction text-nowrap">
            Jurisdiction
          </th>
        <th class="country text-nowrap">
          Linked to
        </th>
          <th class="source text-nowrap">
            Data from
          </th>
      </tr>
    </thead>
    <tbody>
      <tr>
  <td>
    <a href="/nodes/10058123" class="font-weight-bold text-dark">
      NAMCHOW (BRITISH VIRGIN ISLANDS) LTD.
    </a>
  </td>
    <td class="jurisdiction">
      British Virgin Islands
    </td>
  <td class="country">
    Hong Kong
  </td>
    <td class="source text-nowrap">
      <a title="Panama Papers" href="https://www.icij.org/investigations/panama-papers">Panama Papers</a>
    </td>
</tr>
<tr>
  <td>
    <a href="/nodes/191919" class="font-weight-bold text-dark">
      Morgan Howard (British Virgin Islands) Limited
    </a>
  </td>
    <td class="jurisdiction">
      Not identified
    </td>
  <td class="country">
    United Kingdom, Not identified
  </td>
    <td class="source text-nowrap">
      <a title="Offshore Leaks" href="https://www.icij.org/investigations/offshore">Offshore Leaks</a>
    </td>
</tr>
<tr>
  <td>
    <a href="/nodes/82022866" class="font-weight-bold text-dark">
      Hewlett-Packard (British Virgin Islands) Inc.
    </a>
  </td>
    <td class="jurisdiction">
      British Virgin Islands
    </td>
  <td class="country">
    British Virgin Islands
  </td>
    <td class="source text-nowrap">
      <a title="Paradise Papers - Appleby" href="https://www.icij.org/investigations/paradise-papers">Paradise Papers</a>
    </td>
</tr>
<tr>
  <td>
    <a href="/nodes/100636995" class="font-weight-bold text-dark">
      GRANT THORNTON (BRITISH VIRGIN ISLANDS) LIMITED
    </a>
  </td>
    <td class="jurisdiction">
      Barbados
    </td>
  <td class="country">
    Barbados, British Virgin Islands
  </td>
    <td class="source text-nowrap">
      <a title="Paradise Papers - Barbados corporate registry" href="https://www.icij.org/investigations/paradise-papers">Paradise Papers</a>
    </td>
</tr>
<tr>
  <td>
    <a href="/nodes/143158" class="font-weight-bold text-dark">
      The Offshore Institute (British Virgin Islands Branch) Limited
    </a>
  </td>
    <td class="jurisdiction">
      British Virgin Islands
    </td>
  <td class="country">
    British Virgin Islands
  </td>
    <td class="source text-nowrap">
      <a title="Offshore Leaks" href="https://www.icij.org/investigations/offshore">Offshore Leaks</a>
    </td>
</tr>
<tr>
  <td>
    <a href="/nodes/217012" class="font-weight-bold text-dark">
      Galana Africa Ltd.(Continuation in the British Virgin Islands)
    </a>
  </td>
    <td class="jurisdiction">
      Not identified
    </td>
  <td class="country">
    Mauritius, British Virgin Islands
  </td>
    <td class="source text-nowrap">
      <a title="Offshore Leaks" href="https://www.icij.org/investigations/offshore">Offshore Leaks</a>
    </td>
</tr>
<tr>
  <td>
    <a href="/nodes/82009522" class="font-weight-bold text-dark">
      BRITISH VIRGIN ISLANDS CHAMBER OF COMMERCE AND HOTEL ASSOCIATION
    </a>
  </td>
    <td class="jurisdiction">
      British Virgin Islands
    </td>
  <td class="country">
    British Virgin Islands
  </td>
    <td class="source text-nowrap">
      <a title="Paradise Papers - Appleby" href="https://www.icij.org/investigations/paradise-papers">Paradise Papers</a>
    </td>
</tr>
<tr>
  <td>
    <a href="/nodes/240500760" class="font-weight-bold text-dark">
      LORBEL INVESTMENT LTD. (COMP.NO. 1636347), BRITISH VIRGIN ISLANDS
    </a>
  </td>
    <td class="jurisdiction">
      British Virgin Islands
    </td>
  <td class="country">
    
  </td>
    <td class="source text-nowrap">
      <a title="Pandora Papers - Trident Trust" href="https://www.icij.org/investigations/pandora-papers">Pandora Papers</a>
    </td>
</tr>

    </tbody>
  </table>
</div>


Entonces suponog que de eso ya scrapeas y obtienes los atributos: Entity, Jurisdiction, Linked To, Data From.

Entity me pareece que es:
    <a href="/nodes/82009522" class="font-weight-bold text-dark">
      BRITISH VIRGIN ISLANDS CHAMBER OF COMMERCE AND HOTEL ASSOCIATION
    </a>

De ahí EXTRAE EL NOMBRE DE LA ENTIDAD y ese href, porque ese "/nodes/82009522" es muy interesante y lo utilizaré en el front.

Jurisdiction es:

    <td class="jurisdiction">
      British Virgin Islands
    </td>

Linkedt To:

  <td class="country">
    British Virgin Islands
  </td>

Data From:

    <td class="source text-nowrap">
      <a title="Pandora Papers - Trident Trust" href="https://www.icij.org/investigations/pandora-papers">Pandora Papers</a>
    </td>

    Ahí también guarda EL TEXTO O LA INFO Y TAMBIÉN el href también GUARDAMELO!


------------------------------------------------------------------------------------------------------------------------------------------------


AHORA VAMOS A ENTENDER COMO FUNCIONA la fuente:

https://sanctionssearch.ofac.treas.gov/

Atributos: Name, Address, Type, Program(s), List, Score

Ok, APENAS ENTRO A LA PÁGINA ya tenemos o podemos hacer lo siguiente:

Apenas entro a la página, ESPERAS A QUE CARGUE COMPLETAMENTE TODO LO QUE TENGA QUE CARGA.


Hice esto en la consola del navegador:

document.querySelectorAll(".MainTable")

Y solo me retorna esto:

NodeList [table.MainTable]
0: table.MainTable
length: 1

Esa etiqueta tiene eesto:

<table class="MainTable">
                    <tbody><tr>
                        <td class="styleIndent">&nbsp;</td>
                        <td class="Labels"><span id="ctl00_MainContent_lblType" class="fieldHeader" for="ctl00_MainContent_ddlType">Type:</span></td>
                        <td class="styleColumnBody">
                            <select name="ctl00$MainContent$ddlType" id="ctl00_MainContent_ddlType" tabindex="1" title="Select an SDN Type" style="width:205px;">
		<option selected="selected" value="">All</option>
		<option value="Aircraft">Aircraft</option>
		<option value="Entity">Entity</option>
		<option value="Individual">Individual</option>
		<option value="Vessel">Vessel</option>

	</select>
                        </td>
                        <td><span id="ctl00_MainContent_lblAddress" class="fieldHeader" for="ctl00_MainContent_txtAddress">Address:</span></td>
                        <td class="styleColumnBody">
                            <input name="ctl00$MainContent$txtAddress" type="text" maxlength="1000" id="ctl00_MainContent_txtAddress" tabindex="5" title="Enter part of an address as search criteria." style="width:200px;">
                        </td>
                    </tr>
                    <tr>
                        <td class="styleIndent">&nbsp;</td>
                        <td class="Labels"><span id="ctl00_MainContent_lblLastName" class="fieldHeader" for="ctl00_MainContent_txtLastName">Name:</span></td>
                        <td class="styleColumnBody">
                            <input name="ctl00$MainContent$txtLastName" type="text" value="NOMBRE DE LA EMPRESA" maxlength="250" id="ctl00_MainContent_txtLastName" tabindex="2" title="Enter name as search criteria." style="width:200px;">
                        </td>
                        <td class="Labels"><span id="ctl00_MainContent_lblCity" class="fieldHeader" for="ctl00_MainContent_txtCity">City:</span></td>
                        <td class="styleColumnBody">
                            <input name="ctl00$MainContent$txtCity" type="text" maxlength="250" id="ctl00_MainContent_txtCity" tabindex="6" title="Enter city name as search criteria." style="width:200px;">
                        </td>
                    </tr>
                    <tr>
                        <td class="styleIndent">&nbsp;</td>
                        <td class="Labels"><span id="ctl00_MainContent_lblID" class="fieldHeader">ID # / Digital Currency Address:</span></td>
                        <td class="styleColumnBody">
                            <input name="ctl00$MainContent$txtID" type="text" maxlength="250" id="ctl00_MainContent_txtID" tabindex="4" title="Enter ID number or digital currency address as search criteria." style="width:200px;">
                        </td>
                        
                        <td class="Labels"><span id="ctl00_MainContent_lblState" class="fieldHeader" for="ctl00_MainContent_txtState">State/Province:*</span></td>
                        <td class="styleColumnBody">
                            <input name="ctl00$MainContent$txtState" type="text" maxlength="250" id="ctl00_MainContent_txtState" tabindex="7" title="Enter state abbreviation or province name as search criteria." style="width:200px;">
                        </td>
                    </tr>
                    <tr>
                        <td class="styleIndent">&nbsp;</td>
                        <td class="Labels"><span id="ctl00_MainContent_lblProgram" class="fieldHeader" for="ctl00_MainContent_lstPrograms">Program:</span></td>
                        <td class="styleColumnBody">
                            
                                <select size="4" name="ctl00$MainContent$lstPrograms" multiple="multiple" id="ctl00_MainContent_lstPrograms" tabindex="3" class="programList" title="Select one or more sanctions program(s) as search criteria." style="width:205px;">
		<option selected="selected" value="">All</option>
		<option value="561-Related">561-Related</option>
		<option value="BALKANS">BALKANS</option>
		<option value="BALKANS-EO14033">BALKANS-EO14033</option>
		<option value="BELARUS">BELARUS</option>

	</select>
                        </td>
                        <td class="Labels">
                            <div>
                                <span id="ctl00_MainContent_lblCountry" class="fieldHeader" for="ctl00_MainContent_ddlCountry">Country:</span>
                            </div>
                            <div class="stackedLabel">
                                <label for="ctl00_MainContent_ddlList" id="ctl00_MainContent_lblList" class="fieldHeader">List:</label>
                            </div>
                        </td>
                        <td class="styleColumnBody">
                            <div>
                                <select name="ctl00$MainContent$ddlCountry" id="ctl00_MainContent_ddlCountry" tabindex="8" title="Select a country as search criteria." style="width:205px;">
		<option selected="selected" value="">All</option>
		<option value="Afghanistan">Afghanistan</option>
		<option value="Albania">Albania</option>
		<option value="Algeria">Algeria</option>
		<option value="Angola">Angola</option>
	
		<option value="United States">United States</option>
		<option value="Uruguay">Uruguay</option>
		<option value="Uzbekistan">Uzbekistan</option>
		<option value="Vanuatu">Vanuatu</option>
		<option value="Venezuela">Venezuela</option>
		<option value="Vietnam">Vietnam</option>
		<option value="Virgin Islands, British">Virgin Islands, British</option>
		<option value="West Bank">West Bank</option>
		<option value="Yemen">Yemen</option>
		<option value="Zambia">Zambia</option>
		<option value="Zimbabwe">Zimbabwe</option>

	</select>
                            </div>
                            <div class="stackedSelect">
                                <select name="ctl00$MainContent$ddlList" id="ctl00_MainContent_ddlList" tabindex="9" title="Select list(s) as search criteria." style="width:205px;">
		<option selected="selected" value="">All</option>
		<option value="Non-SDN">Non-SDN</option>
		<option value="SDN">SDN</option>

	</select>
                            </div>
                        </td>
                        
                    </tr>
                    <tr>
                        <td class="styleIndent">&nbsp;</td>
                        <td class="Labels"><span id="ctl00_MainContent_lblMinScore" class="fieldHeader">Minimum Name Score:</span></td>
                        <td class="styleColumnBody">
                           <table width="205px" cellpadding="0" cellspacing="0"><tbody><tr>
                            <td><div id="Slider1_railElement" tabindex="-1" class="ajax__slider_h_rail"><div class="ajax__slider_h_handle" title="Select your minimum matching score." style="overflow: hidden; position: absolute; left: 140px;"><img id="Slider1_handleImage" src="WebResource.axd?d=3_RVinVmJovhKfbiF1QGoXBpzmAU7zmBMGrgOgsvHZpRd8DMeDpHHTJKZ_EUmX7PQjQyehXLtRRx9O_2wycbxiybNg2qIxe7dfbymHCiVRsZE1G91fknqdo084XueWVOyGY9dw2&amp;t=637418578420000000"></div></div><input name="ctl00$MainContent$Slider1" type="text" value="100" id="ctl00_MainContent_Slider1" readonly="" style="width: 1px; height: 1px; border: 0px; padding: 0px; margin: 0px; font-size: 1px; line-height: 1px; outline: 0px; position: absolute;"></td>
                            <td style="text-align:right"><input name="ctl00$MainContent$Slider1_Boundcontrol" type="text" value="100" maxlength="5" id="ctl00_MainContent_Slider1_Boundcontrol" title="Enter minimum matching score." style="border-width:1px;border-style:solid;width:25px;"></td>
                            </tr></tbody></table>
                       
                            
                        </td>
                        <td class="Labels"></td>
                        <td style="text-align: left">
                            <input type="submit" name="ctl00$MainContent$btnSearch" value="Search" id="ctl00_MainContent_btnSearch" tabindex="9" style="font-weight:normal;height:22px;width:96px;">&nbsp;&nbsp;
                            <input type="submit" name="ctl00$MainContent$btnReset" value="Reset" id="ctl00_MainContent_btnReset" tabindex="10" style="font-weight:normal;height:22px;width:96px;">
                        </td>
                    </tr>                 
                    
                    
                    
                    
                    
                </tbody></table>



BÁSICAMENTE AQUÍ:

                        <td class="styleColumnBody">
                            <input name="ctl00$MainContent$txtLastName" type="text" value="NOMBRE DE LA EMPRESA" maxlength="250" id="ctl00_MainContent_txtLastName" tabindex="2" title="Enter name as search criteria." style="width:200px;">
                        </td>

VAMOS A BUSCAR el nombre de la empresa, AHÍ LO VAMOS A PONER!

Entonces, una ves se pone el  nombre de la empresa ahí, se da click en el botón "Search":

                        <td style="text-align: left">
                            <input type="submit" name="ctl00$MainContent$btnSearch" value="Search" id="ctl00_MainContent_btnSearch" tabindex="9" style="font-weight:normal;height:22px;width:96px;">&nbsp;&nbsp;
                            <input type="submit" name="ctl00$MainContent$btnReset" value="Reset" id="ctl00_MainContent_btnReset" tabindex="10" style="font-weight:normal;height:22px;width:96px;">
                        </td>

Cuando presiono Search, se recarga la página o se cambia de página creo, pues ya podemos acceder a los resultados:

<div id="ctl00_MainContent_pnlResults" class="Panel">
	

                <div>
                    <table id="resultsHeaderTable" width="920px" style="margin-left:0; margin-right: 0px;">
		<tbody><tr>
			<td style="text-align:left; width:36%">
                                <a id="ctl00_MainContent_btnSortLastName" class="resultHeader" href="javascript:__doPostBack('ctl00$MainContent$btnSortLastName','')" style="color:#000080">Name</a>
                                    
                                    </td>
			<td style="text-align:left; width:30%">
                                <a id="ctl00_MainContent_btnSortAddress" class="resultHeader" href="javascript:__doPostBack('ctl00$MainContent$btnSortAddress','')" style="color:#000080">Address</a>
                                    
                                    </td>
			<td style="text-align:left; width:10%">
                                <a id="ctl00_MainContent_btnSortType" class="resultHeader" href="javascript:__doPostBack('ctl00$MainContent$btnSortType','')" style="color:#000080">Type</a>
                                    
                                    </td>
			<td style="text-align:left; width:10%">
                                <a id="ctl00_MainContent_btnSortPrograms" class="resultHeader" href="javascript:__doPostBack('ctl00$MainContent$btnSortPrograms','')" style="color:#000080">Program(s)</a>
                                    
                                    </td>
			<td style="text-align:left; width:7%">
                                <a id="ctl00_MainContent_btnSortList" class="resultHeader" href="javascript:__doPostBack('ctl00$MainContent$btnSortList','')" style="color:#000080">List</a>
                                    
                                    </td>
			<td style="text-align:left; width:7%">
                                <a id="ctl00_MainContent_btnSortScore" class="resultHeader" href="javascript:__doPostBack('ctl00$MainContent$btnSortScore','')" style="color:#000080">Score</a>
                                    <img id="ctl00_MainContent_imgScore" src="images/desc.gif">
                                    </td>
		</tr>
	</tbody></table>
	
                </div>
                <div id="scrollResults" class="ResultsDiv">
                    
                    <div>
		<table cellspacing="0" id="gvSearchResults" style="width:917px;border-collapse:collapse;">
			<tbody><tr>
				<td style="width:36%;"><a href="Details.aspx?id=26946">MADURO GUERRA, Nicolas Ernesto</a></td>
                <td style="width:30%;">&nbsp;</td>
                <td style="width:10%;">Individual</td>
                <td style="width:10%;">VENEZUELA</td>
                <td style="width:7%;">SDN</td>
                <td style="width:7%;">100</td>
			</tr><tr class="alternatingRowColor">
				<td style="width:36%;"><a href="Details.aspx?id=22790">MADURO MOROS, Nicolas</a></td><td style="width:30%;">&nbsp;</td><td style="width:10%;">Individual</td><td style="width:10%;">IRAN-CON-ARMS-EO; VENEZUELA</td><td style="width:7%;">SDN</td><td style="width:7%;">100</td>
			</tr>
		</tbody></table>
	</div>
                </div>
            
</div>

De ahí ya extraemos los atributos que queremos:

Atributos: Name, Address, Type, Program(s), List, Score

Name:
    <td style="width:36%;"><a href="Details.aspx?id=26946">MADURO GUERRA, Nicolas Ernesto</a></td>

Address:
    <td style="width:30%;">&nbsp;</td>

Type:
    <td style="width:10%;">Individual</td>

Program(s):
    <td style="width:10%;">VENEZUELA</td>

List:
    <td style="width:7%;">SDN</td>

Score:
    <td style="width:7%;">100</td>

De esa forma Y EN ESE ORDEN extrae eesos atribustos y ya está, ya tienes la data.

------------------------------------------------------------------------------------------------------------------------------------------------

AHORA VAMOS A ENTENDER COMO FUNCIONA la fuente:

https://projects.worldbank.org/en/projects-operations/procurement/debarred-firms

Atributos: Firm Name, Address, Country, From Date
(Ineligibility Period), To Date (Ineligibility Period), Grounds.



Apenas entro a la página, ESPERAS A QUE CARGUE COMPLETAMENTE TODO LO QUE TENGA QUE CARGAR, ya que en la página aparece un loading de aprox 5 segundos o si hay una forma de que detectes si automáticamente ya cargó.

Una vez que ya cargó, pues está la tabla:

<div class="kendoTableContainer" bis_skin_checked="1">
    <div class="row" bis_skin_checked="1">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12" bis_skin_checked="1">
            <div class="debarred-firm-header" bis_skin_checked="1"><span>Debarred Firms and Individuals</span> <span class="list-Info">This list updates every 3 hours</span></div>
            <div class="pdf-input-wraper" bis_skin_checked="1">
                <div class="debarred-search" bis_skin_checked="1"> <label>Search : </label> <input type="search" id="category"></div>
                <!-- <div class="pdf-icons-wrap"><span class="font-13"> Download : </span>

<a onclick="exportGridExcel('k-debarred-firms', 'Sanctioned individuals and firms-18-Dec-2025', 'kendoGrid')"><span class="accordion-download"><i class="fa fa-file-excel-o" title="Download Excel" aria-hidden="true"></i></span></a>
<a onclick="exportGridPdf('k-debarred-firms', 'Sanctioned individuals and firms-18-Dec-2025', 'kendoGrid')"><span class="accordion-download"><i class="fa fa-file-pdf-o" title="Download PDF" aria-hidden="true"></i></span></a>   
</div>-->
            </div>
            
            <div id="k-debarred-firms" class="lp_kendo_table k-grid k-widget k-display-block" bis_skin_checked="1" data-role="grid" style=""><div class="k-header k-grid-toolbar" bis_skin_checked="1">Download <a role="button" class="k-button k-button-icontext k-grid-excel" href="#" title="Excel"><span class="k-icon k-i-file-excel"></span></a></div> <div class="k-grid-header" bis_skin_checked="1" style="padding-right: 15px;"><div class="k-grid-header-wrap k-auto-scrollable" bis_skin_checked="1"><table role="grid"><colgroup><col><col><col><col><col><col><col></colgroup><thead role="rowgroup"><tr role="row"><th scope="col" role="columnheader" data-field="SUPP_NAME" aria-haspopup="true" rowspan="2" data-title="Firm Name" data-index="0" id="0c6a2443-51fb-4b3f-a051-4d40ba302623" class="k-header" data-role="columnsorter"><a class="k-link" href="#">Firm Name</a></th><th scope="col" role="columnheader" data-field="ADD_SUPP_INFO" aria-haspopup="true" rowspan="2" data-title="Additional Firm Info" data-index="1" id="9e48d186-9233-4b3f-b92c-22b40c28e157" class="k-header" data-role="columnsorter"><a class="k-link" href="#">Additional Firm Info</a></th><th scope="col" role="columnheader" data-field="SUPPLIER_ADDRESS" aria-haspopup="true" rowspan="2" data-title="Address" data-index="2" id="a2fc11be-ce5b-42b8-afe0-365b4d2afe11" class="k-header" data-role="columnsorter"><a class="k-link" href="#">Address</a></th><th scope="col" role="columnheader" data-field="COUNTRY_NAME" aria-haspopup="true" rowspan="2" data-title="Country" data-index="3" id="41ba3b84-17e2-432d-8cbc-72f3ff1720b0" class="k-header" data-role="columnsorter"><a class="k-link" href="#">Country</a></th><th scope="col" role="columnheader" aria-haspopup="true" colspan="2" data-colspan="2" data-title="Ineligibility Period" id="a5c2d0ee-20b5-4fc2-ac18-adced74b6e56" class="k-header">Ineligibility Period</th><th scope="col" role="columnheader" data-field="DEBAR_REASON" aria-haspopup="true" rowspan="2" data-title="Grounds" data-index="6" id="41a88aab-469c-441b-8c1f-08ab565aab7b" class="k-header" data-role="columnsorter"><a class="k-link" href="#">Grounds</a></th></tr><tr role="row"><th scope="col" role="columnheader" data-field="DEBAR_FROM_DATE" aria-haspopup="true" data-title="From Date" data-index="4" id="3ae4a139-43aa-45b4-a4d8-575a310c5e64" class="k-header k-first k-sorted" data-role="columnsorter" data-dir="desc" aria-sort="descending"><a class="k-link" href="#">From Date<span class="k-icon k-i-sort-desc-sm"></span></a></th><th scope="col" role="columnheader" data-field="DEBAR_TO_DATE" aria-haspopup="true" data-title="To Date" data-index="5" id="1536bc05-1874-4439-b60e-a6aae85a83ba" class="k-header" data-role="columnsorter"><a class="k-link" href="#">To Date</a></th></tr></thead></table></div></div><div class="k-grid-content k-auto-scrollable" bis_skin_checked="1"><table role="grid"><colgroup><col><col><col><col class="k-sorted"><col><col><col></colgroup><tbody role="rowgroup"><tr data-uid="6c5edb67-f506-4c22-9b43-a05e9d70092c" role="row">
            <td class="" role="gridcell">MAX IMPEX LLC</td>
            <td class="" role="gridcell"></td>
            <td class="" role="gridcell">MAX TOWER, JUULCHIN STREET 4/4, CHINGELTEI DISTRICT, ULAANBAATAR 15170</td>
            <td class="" role="gridcell">Mongolia</td>
            <td class="" role="gridcell">16-Dec-2025</td>
            <td class="" role="gridcell">25-May-2030</td>
            <td class="" role="gridcell">Cross Debarment: EBRD</td>
            </tr></tbody></table></div></div>
        </div>
    </div>
</div>


Entonces, yo en:  <input type="search" id="category"></div>

En ese input voy escribiendo, Y POR CADA VEZ QUE CAMBIA EL VALOR DEL INPUT, automaticamnete se van haciendo las busquedas, pero bueno, se supone que yo de frente pondria que el valor final, entonces una vez pongo el valor final, osea la empresa, ESPERO A QUE CARGUE LA INFORMACIÓN y pues ya tendria las respuestas y atributos que buscada:


Firm Name:
<td class="" role="gridcell">MAX IMPEX LLC</td>

Address:
<td class="" role="gridcell">MAX TOWER, JUULCHIN STREET 4/4, CHINGELTEI DISTRICT, ULAANBAATAR 15170</td>

Country:
<td class="" role="gridcell">Mongolia</td>

From Date:
<td class="" role="gridcell">16-Dec-2025</td>

To Date:
<td class="" role="gridcell">25-May-2030</td>

Grounds:
<td class="" role="gridcell">Cross Debarment: EBRD</td>

Y LISTO! YA TENEMOS LA DATA OBTENIDA!





