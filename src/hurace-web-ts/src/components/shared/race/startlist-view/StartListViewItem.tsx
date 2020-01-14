import React from 'react';
import { StartList } from '../../../../models/StartList';
import {
    VerticallyAlignedText,
    ListItem,
    ColumnFlex,
    AlignRight
} from '../../../../theme/CustomComponents';
import styled from 'styled-components';

const StartListItem = styled(ListItem)`
    grid-template-columns: 20px 0.5fr 1fr;
    gap: 24px;
`;

const StartNumber = styled(VerticallyAlignedText)`
    font-weight: bold;
    width: fit-content;
`;

const SkierImage = styled.img`
    border-radius: 50%;
    height: 50px;
    width: 50px;
`;

const NameFlex = styled(ColumnFlex)`
    justify-content: center;
`;

const Country = styled.span`
    color: ${props => props.theme.gray};
    font-style: italic;
    font-size: 12px;
`;

export const StartListViewItem: React.FC<{ startList: StartList }> = ({
    startList
}) => {
    return (
        <StartListItem>
            <StartNumber>{startList.startNumber}</StartNumber>

            <AlignRight>
                <SkierImage src={startList.skier.imageUrl} />
            </AlignRight>

            <NameFlex>
                <span>
                    {startList.skier.firstName} {startList.skier.lastName}
                </span>
                <Country>{startList.skier.country?.countryCode}</Country>
            </NameFlex>
        </StartListItem>
    );
};
