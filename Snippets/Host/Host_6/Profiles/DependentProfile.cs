﻿using NServiceBus;
using NServiceBus.Hosting.Profiles;

#region dependent_profile
class MyProfileHandler :
    IHandleProfile<MyProfile>
{
    public void ProfileActivated(BusConfiguration busConfiguration)
    {
        // set something else in the container
    }
}
#endregion

class MyProfile :
    IProfile
{
}