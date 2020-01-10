import React from 'react';
import { RaceRanking } from '../../../../models/RaceRanking';
import { getTimeWithMSDate } from '../../../../common/timeConverter';
import {
    ListItem,
    VerticallyAlignedText,
    ColumnFlex,
    AlignRight
} from '../../../../theme/CustomComponents';
import styled from 'styled-components';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faBan } from '@fortawesome/free-solid-svg-icons';

const RankingItem = styled(ListItem)`
    display: grid;
    grid-template-columns: 20px 0.5fr 1fr 1fr auto;
    gap: ${props => props.theme.gap};
`;

const Position = styled(VerticallyAlignedText)`
    font-weight: bold;
    width: fit-content;
`;

const SkierImage = styled.img`
    border-radius: 50%;
    height: 50px;
`;

const NameFlex = styled(ColumnFlex)`
    justify-content: center;
`;

const Country = styled.span`
    color: ${props => props.theme.gray};
    font-style: italic;
    font-size: 12px;
`;

export const RankingListViewItem: React.FC<{ raceRanking: RaceRanking }> = ({
    raceRanking
}) => {
    return (
        <RankingItem>
            <Position>
                {raceRanking.disqualified ? (
                    <FontAwesomeIcon icon={faBan} />
                ) : (
                    raceRanking.position
                )}
            </Position>

            <AlignRight>
                <SkierImage src={raceRanking.skier.imageUrl} />
            </AlignRight>
            <NameFlex>
                <span>
                    {raceRanking.skier.firstName} {raceRanking.skier.lastName}
                </span>
                <Country>{raceRanking.skier.country?.countryCode}</Country>
            </NameFlex>

            <VerticallyAlignedText>
                {raceRanking.disqualified
                    ? '-'
                    : getTimeWithMSDate(raceRanking.time!)}
            </VerticallyAlignedText>
            <VerticallyAlignedText>
                {raceRanking.disqualified
                    ? '-'
                    : `+${getTimeWithMSDate(raceRanking.timeToLeader!)}`}
            </VerticallyAlignedText>
        </RankingItem>
    );
};
