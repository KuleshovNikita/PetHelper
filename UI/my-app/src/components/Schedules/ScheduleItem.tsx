import { TextField, TextFieldProps, Typography } from "@mui/material";
import { TimePicker } from "@mui/x-date-pickers";
import React, { useState } from "react";
import { WalkingSchedule } from "../../models/Pet";

type Props = {
    scheduleItem: WalkingSchedule
}

const timePickerStyles = {
    width: 120,
    mr: 1
}

export default function ScheduleItem({ scheduleItem }: Props) {

    const [isTextFieldDisabled, setTextFieldDisabled] = useState(true);

    const [start, setStart] = useState<Date | null>(new Date(scheduleItem.scheduledStart));
    const [end, setEnd] = useState<Date | null>(new Date(scheduleItem.scheduledEnd));

    const configureTextField = (props: TextFieldProps) => {
        return (
            <TextField 
                {...props} 
                sx={timePickerStyles}
                size="small"
            />
        );
    }

    return (
        <Typography 
            variant="h4" 
            component="h4"
            color="white">
                Start: <TimePicker
                            value={start}
                            onChange={(value) => { setStart(value) }}
                            renderInput={configureTextField}
                            ampm={false}
                        />
                End: <TimePicker
                            value={end}
                            onChange={(value) => { setEnd(value) }}
                            renderInput={configureTextField}
                            ampm={false}
                        />
        </Typography>
    );
}