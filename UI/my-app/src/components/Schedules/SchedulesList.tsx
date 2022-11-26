import { Button, ListItem, ListItemButton, ListItemText, TextField, Typography } from "@mui/material";
import React, { Component, useState } from "react";
import { WalkingSchedule } from "../../models/Pet";
import AddIcon from '@mui/icons-material/Add';
import ScheduleItem from "./ScheduleItem";

type Props = {
    schedules: WalkingSchedule[]
}

const listItemStyle = {
    bgcolor: "orange",
    borderRadius: "5px",
    mb: 1
}

const addScheduleButton = {
    "&:hover": {
        bgcolor: "brown"
    },
    bgcolor: "orange", 
    color: "white"
}

export default function SchedulesList({ schedules }: Props) {
    return(
        <>
            <Typography variant="h4" component="h4">
                Schedules:
            </Typography>
                {
                    schedules && 
                    schedules.length !== 0 &&
                    schedules.map((sch, key) => (
                        <ListItem key={key} component="a" sx={listItemStyle}>
                            <ScheduleItem scheduleItem={sch}/>
                        </ListItem>
                    ))
                }
                
            <Button sx={addScheduleButton}>
                <AddIcon/>
            </Button>
        </>
    );
}