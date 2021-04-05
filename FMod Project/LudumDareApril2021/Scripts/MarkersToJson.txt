/* -------------------------------------------
   FMOD Studio Script:
   NamedMarkers to JSON file
   -------------------------------------------
 */
studio.menu.addMenuItem({
    name: "MPP\\ Markers to JSON",
    isEnabled: function() { return studio.window.browserSelection().length; },
    execute: function() {

        var song_markers = []
        var event = studio.window.browserCurrent(); //get fmod selected event

        marker_tracks = event.markerTracks // get markers
        marker_tracks.forEach(function(track) {
            track.markers.forEach(function(marker) {
                if(marker.entity === "NamedMarker") {
                    song_markers.push({
                        position: marker.position,
                        name: marker.name
                    })
                }
            })
        })

        song_markers = song_markers.sort(function(a, b) {
            return parseFloat(a.position) - parseFloat(b.position);
        });

        console.log(JSON.stringify(song_markers))
        
        var path = studio.project.filePath
        path = path.substring(0, path.lastIndexOf("/"));

        var file = studio.system.getFile(path+"/"+event.name+".json")
        console.log(file.open(studio.system.openMode.ReadWrite))
        file.writeText(JSON.stringify(song_markers));
        file.close()
    },
});