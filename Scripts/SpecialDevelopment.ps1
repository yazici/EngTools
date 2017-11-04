$($MyInvocation.MyCommand.Source)

# mouse on a region and click f5 it goes to other side...
	# Blender Development: "https://wiki.blender.org/index.php/Dev:Contents"
	# "Blender Source Code Git Commits": "https://developer.blender.org/diffusion/"
	
	## SPACE_INFO 
    # https://wiki.blender.org/index.php/Dev:Source/UI/Tutorials/AddAnEditor#Looking_at_the_Source 
    # https://svn.blender.org/svnroot/bf-blender/trunk/blender/source/blender/editors/space_info/

	# bpy.ops.screen.header_flip()
	# bpy.ops.screen.region_flip()
	# bpy.ops.screen.header_toggle_menus()

function CloneBlenderSourceCodeReadWrite() 
{
	git clone git@git.blender.org:blender.git
}

function CloneBlenderSourceCodeReadOnly() 
{
	git clone git://git.blender.org/blender.git
}

function SomeBlenderTooling() {
@"
	https://cloud.blender.org/blog/attract-and-flamenco-public-beta

	Attract: production tracking
	
	Attract is the production tracking software we used while working on Agent 327: 
	Operation Barbershop and Cosmos Laundromat. It currently offers task management,
	with multiple user assignments, due dates, activity log and comments. 
	Tasks can be managed with asset and shot lists.
"@

@" 
	Flamenco: render management
	
	Flamenco is our render managements software, and much more. We have been using it 
	since Cosmos Laundromat to render animations, OpenGL previews, simulations and so on. 
"@


@"
	http://www.alembic.io/index.html

	Alembic is an open computer graphics interchange framework. 
	
	Alembic distills complex, animated scenes into a non-procedural, 
	application-independent set of baked geometric results. This �distillation' 
	of scenes into baked geometry is exactly analogous to the distillation of 
	lighting and rendering scenes into rendered image data.
	
Alembic...
	...Is a data representation scheme for storing computer graphics scenes 
	...Distills the results of artist disciplines for handoff to other artists in other disciplines 
	...Is focused on the greatest common divisor between applications, the 'periodic table of cg primitives' 
	...Is extensible to support new workflows and new tools 

Alembic Is Not...
	...A dependency graph, nor a procedural data transformation tool 
	...A replacement for native application scene file formats 
	...An asset management application 
	...A general rigging storage solution 

Alembic Would Be Used...
	...To bake the results of an animated scene for hand-off to lighting & rendering 
	...To hand off an animated creature for cloth or flesh simulation 
	...To store the results of a cloth or flesh simulation for use in lighting & rendering 
	...To hand off animated geometry to a physical simulation engine 
	...To store the results of a physical simulation engine for use in lighting & rendering 

Alembic Would Not Be Used...
	...To transport complex procedural animation rigs between different applications 
	...To make lossless round trips out of and into the same computation context 
	...To construct complex networks of procedural tools 
"@
}
