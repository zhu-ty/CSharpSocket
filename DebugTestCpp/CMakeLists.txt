cmake_minimum_required (VERSION 3.10)
set(name "socket")
project ( ${name} )

#cuda
find_package(CUDA)
set(CUDA_NVCC_FLAGS
    ${CUDA_NVCC_FLAGS}
#    -O3 
	-gencode=arch=compute_61,code=\"sm_61,compute_61\"
    )

set(CMAKE_CXX_FLAGS "/clr /FU\"C:\\Program Files (x86)\\Reference Assemblies\\Microsoft\\Framework\\.NETFramework\\v4.0\\mscorlib.dll\"")
if(CMAKE_CXX_FLAGS_DEBUG MATCHES "/RTC1")
   string(REPLACE "/RTC1" " " CMAKE_CXX_FLAGS_DEBUG "${CMAKE_CXX_FLAGS_DEBUG}")
endif()
if(CMAKE_CXX_FLAGS MATCHES "/EHsc")
   string(REPLACE "/EHsc" "" CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS}")
endif()

cuda_add_executable(${name}
    ./DebugTestCpp/main.cpp
)