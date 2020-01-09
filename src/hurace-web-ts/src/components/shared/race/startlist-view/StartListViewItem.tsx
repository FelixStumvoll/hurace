import React from 'react';
import { StartList } from '../../../../models/StartList';
import {
    Card,
    VerticallyAlignedText,
    AlignRight
} from '../../../../theme/CustomComponents';
import styled from 'styled-components';

const ListItem = styled(Card)`
    padding: 10px;
    border-radius: 0;
    display: grid;
    grid-template-columns: 30px 1fr 1fr auto;
    gap: 24px;
`;

const StartNumber = styled(VerticallyAlignedText)`
    font-weight: bold;
`;

const SkierImage = styled.img`
    border-radius: 50%;
    height: 50px;
`;

export const StartListViewItem: React.FC<{ startList: StartList }> = ({
    startList
}) => {
    return (
        <ListItem>
            <StartNumber>{startList.startNumber}</StartNumber>
            <SkierImage src={startList.skier.imageUrl} />

            <VerticallyAlignedText>
                {startList.skier.firstName} {startList.skier.lastName}
            </VerticallyAlignedText>
            <VerticallyAlignedText>
                {startList.skier.country?.countryCode}
            </VerticallyAlignedText>
        </ListItem>
    );
};
