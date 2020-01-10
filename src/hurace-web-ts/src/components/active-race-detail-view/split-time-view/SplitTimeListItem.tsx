import React from 'react';
import { TimeDifference } from '../../../models/TimeDifference';
import styled from 'styled-components';
import {
    ListItem,
    VerticallyAlignedText
} from '../../../theme/CustomComponents';
import { getTimeWithMSDate } from '../../../common/timeConverter';

const SplitTimeItem = styled(ListItem)`
    display: grid;
    grid-template-columns: 20px 1fr 70px 82px;
    gap: ${props => props.theme.gap};
`;

export const SplitTimeListItem: React.FC<{ timeDiff: TimeDifference }> = ({
    timeDiff
}) => {
    return (
        <SplitTimeItem>
            <VerticallyAlignedText>
                {timeDiff.sensorNumber}
            </VerticallyAlignedText>
            <div></div>
            <VerticallyAlignedText>
                {getTimeWithMSDate(new Date(timeDiff.time))}
            </VerticallyAlignedText>
            <VerticallyAlignedText>
                {timeDiff.differenceToLeader < 0 ? '-' : '+'}
                {getTimeWithMSDate(
                    new Date(Math.abs(timeDiff.differenceToLeader))
                )}
            </VerticallyAlignedText>
        </SplitTimeItem>
    );
};
