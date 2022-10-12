# Baby & breastfeeding tracker

## Wat is het doel van de app?
* Stats van de baby(‘s) tracken
    * Gewicht en lengte tracken, en evolutie weergeven in grafiek over tijd
    * Belangrijke gebeurtenissen via video of foto
* Hulp &amp; stats omtrent borstvoeding
    * Timer implementeren die borstvoeding trackt: hoelang en aan welke tepels
    * Timer implementeren die kolven trackt: hoe lang, aan welke tepels en aantal cc melk
    * De moeder notifiëren wanneer borstvoeding of kolven moet starten: deze tijdsintervallen kan je instellen
* Memories (foto’s en videos) met een comment bijhouden in de app

## Wie moet het gebruiken?
* Moeders met een baby.
* Functioneel op Android + UWP

## Hoe gaat de gebruiker er mee aan de slag?
* De moeder registreert zichzelf eerst: een profiel aanmaken van zichzelf en van de baby met
een aantal startgegevens (gewicht, lengte, foto =&gt; kiezen uit gallery of in app camera).
Nadien kan men inloggen met email en paswoord.
* De startgegevens zijn meteen zichtbaar in de statistieken.
* Er kunnen in de app nog meer baby’s toegevoegd worden. De statistieken worden dan in
verschillende tabs getoond.

## De online strategie
Online CRUD operaties met een backend service: Firebase

## De mobile features
* Xamarin essentials
    * Media picker:
        * Foto van baby(‘s) en moeder maken of kiezen uit gallery
        * Videos opnemen van ‘memorable’ gebeurtenissen die je later opnieuw wil bekijken, en deze tonen in een apart tabblad 
* Geolocation &amp; maps:
    * Vanaf de huidige locatie wordt er een zoekopdracht op google maps gedaan naar de dichtstbijzijnde winkels waar men luiers verkoopt (in nood kan je deze functie raadplegen)

* Phone dialer: nummer van ingegeven vroedvrouw kan steeds gebeld worden via de
app (indien advies nodig is)

* Push notifications
    * wanneer moet er borstvoeding of kolven plaatsvinden
* 2D graphics
    * De stats (van baby en borstvoeding) in grafiekjes gieten en die over de tijd plotten
